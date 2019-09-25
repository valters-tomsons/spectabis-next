using SpectabisLib.Configuration;
using SpectabisUI.Configuration;

namespace SpectabisUI.Interfaces
{
    public interface IConfigurationLoader
    {
        SpectabisConfig Spectabis { get; }
        UIConfiguration UserInterface { get; }
    }
}