using System.IO;

namespace SpectabisLib.Helpers
{
    public static class InitializeFileSystem
    {
        public static void Initialize()
        {
            CreateConfigurationDirectory();
        }

        private static void CreateConfigurationDirectory()
        {
            if(Directory.Exists(SystemDirectories.ConfigFolder))
            {
                return;
            }

            Directory.CreateDirectory(SystemDirectories.ConfigFolder);
        }
    }
}