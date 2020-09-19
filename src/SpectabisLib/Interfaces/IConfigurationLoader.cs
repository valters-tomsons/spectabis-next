using System.Threading.Tasks;
using SpectabisLib.Configuration;

namespace SpectabisLib.Interfaces
{
    public interface IConfigurationLoader
    {
        SpectabisConfig Spectabis { get; }
        UIConfig UserInterface { get; }
        DirectoryConfig Directories { get; }
        Task WriteConfiguration<T>(T obj) where T : IJsonConfig, new();
        Task<T> ReadConfiguration<T>() where T : IJsonConfig, new();
        bool ConfigurationExists<T>() where T : IJsonConfig, new();
        Task WriteDefaultsIfNotExist<T>() where T : IJsonConfig, new();
    }
}