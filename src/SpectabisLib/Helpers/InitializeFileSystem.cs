using System.IO;

namespace SpectabisLib.Helpers
{
    public static class InitializeFileSystem
    {
        public static void Initialize()
        {
            CreateConfigurationDirectory();
            CreateProfileDirectory();
        }

        private static void CreateConfigurationDirectory()
        {
            if(Directory.Exists(SystemDirectories.ConfigFolder))
            {
                return;
            }

            Directory.CreateDirectory(SystemDirectories.ConfigFolder);
        }

        private static void CreateProfileDirectory()
        {
            if(Directory.Exists(SystemDirectories.ProfileFolder))
            {
                return;
            }

            Directory.CreateDirectory(SystemDirectories.ProfileFolder);
        }
    }
}