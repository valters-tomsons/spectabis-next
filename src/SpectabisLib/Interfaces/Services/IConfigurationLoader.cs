using System.Threading.Tasks;
using SpectabisLib.Configuration;

namespace SpectabisLib.Interfaces
{
    public interface IConfigurationLoader
    {
        SpectabisConfig Spectabis { get; set; }
        UIConfig UserInterface { get; set; }
        DirectoryConfig Directories { get; set; }
        TextConfig TextConfig { get; set; }

        Task WriteConfiguration<T>(T obj) where T : IJsonConfig, new();
        Task<T> ReadConfiguration<T>() where T : IJsonConfig, new();
        bool ConfigurationExists<T>() where T : IJsonConfig, new();
        Task WriteDefaultsIfNotExist<T>() where T : IJsonConfig, new();
    }
}