using System.IO;

namespace SpectabisLib.Helpers
{
    public static class InitializeFileSystem
    {
        public static void Initialize()
        {
            Directory.CreateDirectory(SystemDirectories.ConfigFolder);
            Directory.CreateDirectory(SystemDirectories.ProfileFolder);
            Directory.CreateDirectory(SystemDirectories.GlobalConfigsFolder);
        }
    }
}