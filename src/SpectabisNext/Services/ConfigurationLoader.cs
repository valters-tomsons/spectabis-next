using SpectabisLib.Configuration;
using SpectabisUI.Configuration;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; }
        public UIConfiguration UserInterface { get; }

        public ConfigurationLoader()
        {
           UserInterface = new UIConfiguration();
           Spectabis = new SpectabisConfig();
        }
    }
}