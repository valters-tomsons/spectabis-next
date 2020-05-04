using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SpectabisLib.Configuration;
using SpectabisLib.Interfaces;
using SpectabisUI.Configuration;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; }
        public UIConfiguration UserInterface { get; }
        public DirectoryStruct Directories { get; private set; }

        public ConfigurationLoader()
        {
            UserInterface = new UIConfiguration();
            Spectabis = new SpectabisConfig();
            Directories = new DirectoryStruct();
        }
    }
}