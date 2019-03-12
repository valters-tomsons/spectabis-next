using System.IO;
using Avalonia.Media.Imaging;
using SpectabisNext.Interfaces;

namespace SpectabisNext.Services
{
    public class BitmapLoader : IBitmapLoader
    {
        private readonly ConfigurationLoader _configurationLoader;

        public Bitmap DefaultBoxart { get; private set; }

        public BitmapLoader(ConfigurationLoader configurationLoader)
        {
            DefaultBoxart = LoadDefaultBoxart();
            _configurationLoader = configurationLoader;
        }

        public Bitmap LoadFromFile(string filePath)
        {
            if(File.Exists(filePath))
            {
                return new Bitmap(filePath);
            }

            return DefaultBoxart;
        }

        private Bitmap LoadDefaultBoxart()
        {
            //null because i have no idea on how i'll handle embedded resources
            return null;
        }

    }
}