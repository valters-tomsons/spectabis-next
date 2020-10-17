using System.Text;
using System.IO;
using System;
using System.Linq;
using EmuConfig.Interfaces;

namespace EmuConfig.Abstractions
{
    public class IniParser : IConfigParser
    {
        public T ReadConfig<T>(Uri iniPath) where T : IConfigurable, new()
        {
            var parser = new IniFileParser.IniFileParser();
            var data = parser.ReadFile(iniPath.OriginalString);

            var dict = data.Global.ToDictionary(x => x.KeyName, y => y.Value);

            return new T() { IniData = dict };
        }

        public void WriteConfig<T>(Uri iniPath, T config) where T : IConfigurable
        {
            var keys = config.GetConfigKeys();

            var iniContent = File.ReadAllLines(iniPath.LocalPath);

            for(int i = 0; i < iniContent.Length; i++)
            {
                var content = iniContent[i];

                foreach(var key in keys)
                {
                    if(content.StartsWith(key))
                    {
                        var resultLine = $"{key} = {config[key]}";
                        iniContent[i] = resultLine;
                        Console.WriteLine(resultLine);
                    }
                }
            }

            File.WriteAllLines(iniPath.LocalPath, iniContent, Encoding.ASCII);
        }
    }
}