using System.Linq;
using System;
using System.Collections.Generic;
using EmuConfig.Attributes;
using EmuConfig.Interfaces;

namespace EmuConfig.Abstractions
{
    public class IniConfiguration : IConfigurable
    {
        public string this[string iniKey]
        {
            get => GetValueFromIniKey(iniKey);
        }

        public string[] GetConfigKeys()
        {
            var keyProperties = GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(IniKeyAttribute)));
            var attributes = keyProperties.Select(x => Attribute.GetCustomAttribute(x, typeof(IniKeyAttribute)) as IniKeyAttribute);
            var keys = attributes.Select(x => x?.GetKey()).Where(x => !string.IsNullOrWhiteSpace(x));
            return keys.ToArray()!;
        }

        private IDictionary<string, string> iniData = new Dictionary<string, string>();
        public IDictionary<string, string> IniData { get => iniData; set => FromIniData(value); }

        private string GetValueFromIniKey(string iniKey)
        {
            var attributedProperties = GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(IniKeyAttribute)));

            foreach(var property in attributedProperties)
            {
                var member = Attribute.GetCustomAttribute(property, typeof(IniKeyAttribute)) as IniKeyAttribute;

                if(member?.GetKey() != iniKey)
                {
                    continue;
                }

                var propertyName = property.Name;
                var valueObject = GetValueByPropertyName(propertyName);

                if(property.PropertyType.IsEnum)
                {
                    var enumObject = valueObject as Enum;
                    var result = Convert.ToInt32(enumObject);
                    return $"{result}";
                }

                return valueObject.ToString();
            }

            throw new InvalidOperationException("Ini configuration is empty");
        }

        private void FromIniData(IDictionary<string, string> data)
        {
            iniData = data;
            MapPropertiesFromDictionary(iniData);
        }

        private void MapPropertiesFromDictionary(IDictionary<string, string> data)
        {
            foreach(var prop in GetType().GetProperties())
            {
                var isKey = Attribute.IsDefined(prop, typeof(IniKeyAttribute));

                if(!isKey)
                {
                    continue;
                }

                var member = Attribute.GetCustomAttribute(prop, typeof(IniKeyAttribute)) as IniKeyAttribute;
                var propKey = member?.GetKey();

                if(propKey is null || !data.ContainsKey(propKey))
                {
                    continue;
                }

                var stringValue = data[propKey];

                var isInt = int.TryParse(stringValue, out int intValue);
                var isEnum = prop.PropertyType.IsEnum;
                if (isInt && isEnum)
                {
                    var enumObject = Enum.ToObject(prop.PropertyType, intValue);
                    SetValueByPropertyName(prop.Name, enumObject);
                    continue;
                }

                if(isInt)
                {
                    SetValueByPropertyName(prop.Name, intValue);
                }
                else{
                    SetValueByPropertyName(prop.Name, data[propKey]);
                }
            }
        }

        private void SetValueByPropertyName(string propertyName, object value)
        {
            GetType().GetProperty(propertyName).SetValue(this, value, null);
        }

        private object GetValueByPropertyName(string propertyName)
        {
            return GetType().GetProperty(propertyName).GetValue(this, null);
        }
    }
}