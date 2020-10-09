using System.Collections.Generic;

namespace EmuConfig.Interfaces
{
    public interface IConfigurable
    {
        IDictionary<string, string> IniData { get; set; }
    }
}