using System;
using System.Linq;
using EmuConfig.Interfaces;

namespace EmuConfig.Abstractions
{
    public class IniParser
    {
        public T ReadConfig<T>(Uri iniPath) where T : IConfigurable, new()
        {
            var parser = new IniFileParser.IniFileParser();
            var data = parser.ReadFile(iniPath.OriginalString);

            var dict = data.Global.ToDictionary(x => x.KeyName, y => y.Value);

            return new T() { IniData = dict };
        }
    }
}