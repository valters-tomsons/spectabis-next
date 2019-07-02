using SpectabisLib.Configuration;
using SpectabisUI.Interfaces;
using SpectaibsUI.Configuration;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
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