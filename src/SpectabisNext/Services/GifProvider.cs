using System.Collections.Generic;
using Avalonia.Controls;
using SpectabisUI.Controls.AnimatedImage;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;
using SpectabisLib.Helpers;

namespace SpectabisNext.Services
{
    public class GifProvider : IGifProvider
    {
        private readonly Dictionary<GameProfile, GifInstance> _instances;

        public GifProvider()
        {
            _instances = new Dictionary<GameProfile, GifInstance>();
        }

        public void StartSpinner(GameProfile game, Image tile)
        {
            var spinner = $"{SystemDirectories.ResourcesPath}/Images/spinner.gif";

            var instance = CreateGifInstance(tile);
            _instances.Add(game, instance);
            instance.SetSource(spinner);
        }

        public void DisponseSpinner(GameProfile game)
        {
            if(_instances.TryGetValue(game, out var instance))
            {
                _instances.Remove(game);
                instance.Dispose();
            }
        }

        private static GifInstance CreateGifInstance(Image targetControl)
        {
            return new GifInstance() { TargetControl = targetControl };
        }
    }
}