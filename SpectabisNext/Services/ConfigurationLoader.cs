using SpectabisLib.Configuration;
using SpectabisNext.Configuration;
using SpectabisNext.Interfaces;

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