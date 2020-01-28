using System.IO;
using Avalonia.Media.Imaging;
using SpectabisLib;
using SpectabisLib.Helpers;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class BitmapLoader : IBitmapLoader
    {
        public Bitmap DefaultBoxart { get; }

        public BitmapLoader()
        {
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