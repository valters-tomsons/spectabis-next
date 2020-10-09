using System;
using System.Collections.Generic;
using EmuConfig.Attributes;
using EmuConfig.Interfaces;

namespace EmuConfig.Bases
{
    public class IniConfiguration : IConfigurable
    {
        public object this[string iniKey]
        {
            get { return GetType().GetProperty(iniKey).GetValue(this, null); }
            set { GetType().GetProperty(iniKey).SetValue(this, value, null); }
        }

        private IDictionary<string, string> iniData;
        public IDictionary<string, string> IniData { get => iniData; set => MapData(value); }

        private void MapData(IDictionary<string, string> data)
        {
            iniData = data;
            MapProperties(iniData);
        }

        private void MapProperties(IDictionary<string, string> data)
        {
            var props = GetType().GetProperties();

            foreach(var prop in props)
            {
                var isKey = Attribute.IsDefined(prop, typeof(IniKeyAttribute));

                if(!isKey)
                {
                    continue;
                }

                var member = Attribute.GetCustomAttribute(prop, typeof(IniKeyAttribute)) as IniKeyAttribute;
                var propKey = member.GetKey();

                var dataMatch = data.ContainsKey(propKey);

                if(!dataMatch)
                {
                    continue;
                }

                var value = data[propKey];

                var isInt = int.TryParse(value, out int intValue);
                var isEnum = prop.PropertyType.IsEnum;
                if (isInt && isEnum)
                {
                    this[prop.Name] = Enum.ToObject(prop.PropertyType, intValue);
                    continue;
                }

                prop.SetValue(prop, value);
            }
        }
    }
}