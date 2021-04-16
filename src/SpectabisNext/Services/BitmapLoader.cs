using System;
using System.IO;
using Avalonia.Media.Imaging;
using SpectabisLib.Abstractions;
using SpectabisLib.Helpers;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Services
{
    public class BitmapLoader : IBitmapLoader
    {
        public Bitmap DefaultBoxart { get; }
        private readonly ProfileFileSystem _profileFs;

        public BitmapLoader(ProfileFileSystem profileFs)
        {
            _profileFs = profileFs;
            DefaultBoxart = LoadDefaultBoxart();
        }

        public Bitmap GetBoxArt(GameProfile game)
        {
            if (!string.IsNullOrWhiteSpace(game.BoxArtPath))
            {
                return new Bitmap(game.BoxArtPath);
            }

            var boxArtPath = _profileFs.GetBoxArtPath(game);

            if (boxArtPath == null || !File.Exists(boxArtPath.LocalPath))
            {
                return DefaultBoxart;
            }

            try
            {
                return new Bitmap(boxArtPath.LocalPath);
            }
            catch(Exception e)
            {
                Logging.WriteLine($"Failed to load boxart for '{game.Id}'");
                Logging.WriteLine(e.Message);

                return DefaultBoxart;
            }
        }

        private Bitmap LoadDefaultBoxart()
        {
            var tempArtPath = $"{SystemDirectories.ResourcesPath}/Images/placeholderBoxart.jpg";

            if (File.Exists(tempArtPath))
            {
                return new Bitmap(tempArtPath);
            }

            Logging.WriteLine(Path.GetFullPath(tempArtPath));
            Logging.WriteLine($"Cannot locate {tempArtPath}");
            Logging.WriteLine("Failed to load default boxart");
            return null;
        }
    }
}