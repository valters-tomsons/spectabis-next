using System;
using System.Threading.Tasks;

namespace EmuConfig.Interfaces
{
    public interface IParserProvider
    {
        T ReadConfig<T>(Uri iniPath) where T : IConfigurable, new();
        Task WriteConfig<T>(Uri iniPath, T config) where T : IConfigurable;
    }
}