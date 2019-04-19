using SpectabisLib.Configuration;
using SpectaibsUI.Configuration;

namespace SpectabisUI.Interfaces
{
    public interface IConfigurationLoader
    {
        SpectabisConfig Spectabis { get; }
        UIConfiguration UserInterface { get; }
    }
}