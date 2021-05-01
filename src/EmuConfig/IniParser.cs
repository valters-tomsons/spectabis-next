using System;
using System.IO;
using System.Text;
using System.Linq;
using EmuConfig.Interfaces;
using System.Threading.Tasks;

namespace EmuConfig
{
    public class IniParser : IParserProvider
    {
        public T ReadConfig<T>(Uri iniPath) where T : IConfigurable, new()
        {
            var parser = new IniFileParser.IniFileParser();
            var data = parser.ReadFile(iniPath.OriginalString, Encoding.ASCII);

            var dict = data.Global.ToDictionary(x => x.KeyName, y => y.Value);

            return new T() { IniData = dict };
        }

        public async Task WriteConfig<T>(Uri iniPath, T config) where T : IConfigurable
        {
            var keys = config.GetConfigKeys();

            var iniContent = await File.ReadAllLinesAsync(iniPath.LocalPath).ConfigureAwait(false);

            for(int i = 0; i < iniContent.Length; i++)
            {
                var content = iniContent[i];

                foreach(var key in keys)
                {
                    if(content.StartsWith(key))
                    {
                        iniContent[i] = $"{key} = {config[key]}";
                    }
                }
            }

            File.Delete(iniPath.LocalPath);
            await File.WriteAllLinesAsync(iniPath.LocalPath, iniContent, Encoding.ASCII).ConfigureAwait(false);
        }
    }
}