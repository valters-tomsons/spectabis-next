using System;

namespace EmuConfig.Interfaces
{
    public interface IConfigParser
    {
        T ReadConfig<T>(Uri iniPath) where T : IConfigurable, new();
        void WriteConfig<T>(Uri iniPath, T config) where T : IConfigurable;
    }
}