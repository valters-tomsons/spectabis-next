using SpectabisLib.Configuration;
using SpectabisUI.Configuration;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : SpectabisUI.Interfaces.IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; private set; }
        public UIConfiguration UserInterface { get; private set; }

        public ConfigurationLoader()
        {
           UserInterface = new UIConfiguration(); 
           Spectabis = new SpectabisConfig();
        }
    }
}