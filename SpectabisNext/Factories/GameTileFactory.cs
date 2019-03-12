using SpectabisLib.Models;
using SpectabisNext.Configuration;
using SpectabisNext.Controls.GameTile;
using SpectabisNext.Interfaces;
using SpectabisNext.Services;

namespace SpectabisNext.Factories
{
    public class GameTileFactory
    {
        private readonly IConfigurationLoader _configuration;
        private readonly IBitmapLoader _bitmapLoader;

        public GameTileFactory(ConfigurationLoader configurationLoader, BitmapLoader bitmapLoader)
        {
            _configuration = configurationLoader;
            _bitmapLoader = bitmapLoader;
        }

        public GameTileView Create(GameProfile game)
        {
            var sizeModifier = _configuration.UserInterface.BoxArtSizeModifier;
            var tileGapSize = _configuration.UserInterface.BoxArtGapSize;
            var boxartBitmap = _bitmapLoader.LoadFromFile(game.BoxArtPath);

            var tileView = new GameTileView(game)
            {
                Height = _configuration.UserInterface.BoxArtHeight * sizeModifier,
                Width = _configuration.UserInterface.BoxArtWidth * sizeModifier,
                Margin = new Avalonia.Thickness(0, 0, tileGapSize, tileGapSize),
            };

            tileView.LoadBoxart(boxartBitmap);

            return tileView;
        }
    }
}