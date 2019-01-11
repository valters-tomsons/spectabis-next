using SpectabisLib.Configuration;
using SpectabisNext.Configuration;

namespace SpectabisNext.Interfaces
{
    public interface IConfigurationLoader
    {
        SpectabisConfig Spectabis { get; }
        UIConfiguration UserInterface { get; }
    }
}