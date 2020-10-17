using System.Collections.Generic;

namespace EmuConfig.Interfaces
{
    public interface IConfigurable
    {
        string this[string iniKey] { get; }

        IDictionary<string, string> IniData { get; set; }

        string[] GetConfigKeys();
    }
}