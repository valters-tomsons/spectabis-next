using System.IO;
using Avalonia.Media.Imaging;
using SpectabisLib;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Services
{
    public class BitmapLoader : IBitmapLoader
    {
        private readonly ConfigurationLoader _configurationLoader;

        public Bitmap DefaultBoxart { get; private set; }

        public BitmapLoader(ConfigurationLoader configurationLoader)
        {
            _configurationLoader = configurationLoader;
            DefaultBoxart = LoadDefaultBoxart();
        }

        public Bitmap LoadFromFile(string filePath)
        {
            if(File.Exists(filePath))
            {
                return new Bitmap(filePath);
            }

            return null;
        }

        private Bitmap LoadDefaultBoxart()
        {
            var tempArtPath = $"{SystemDirectories.ResourcesPath}/Images/placeholderBoxart.jpg";
            var fullPath = Path.GetFullPath(tempArtPath);

            if(File.Exists(tempArtPath))
            {
                return new Bitmap(tempArtPath);
            }

            System.Console.WriteLine(Path.GetFullPath(tempArtPath));
            System.Console.WriteLine($"Cannot locate {tempArtPath}");
            System.Console.WriteLine("Failed to load default boxart");
            return null;
        }

    }
}