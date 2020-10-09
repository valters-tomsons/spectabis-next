using System;

namespace EmuConfig.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IniKeyAttribute : Attribute
    {
        private readonly string iniKey;

        public IniKeyAttribute(string key)
        {
            iniKey = key;
        }

        public string GetKey()
        {
            return iniKey;
        }
    }
}