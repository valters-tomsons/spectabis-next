using SpectabisLib.Models;
using SpectabisNext.Controls.GameTileView;
using SpectabisNext.Services;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Factories
{
    public class GameTileFactory
    {
        private readonly IConfigurationLoader _configuration;
        private readonly IBitmapLoader _bitmapLoader;

        public GameTileFactory(IConfigurationLoader configurationLoader, IBitmapLoader bitmapLoader)
        {
            _configuration = configurationLoader;
            _bitmapLoader = bitmapLoader;
        }

        public GameTileView Create(GameProfile game)
        {
            var sizeModifier = _configuration.UserInterface.BoxArtSizeModifier;
            var tileGapSize = _configuration.UserInterface.BoxArtGapSize;
            var boxartBitmap = _bitmapLoader.LoadFromFile(game.BoxArtPath);

            if(boxartBitmap == null)
            {
                boxartBitmap = _bitmapLoader.DefaultBoxart;
            }

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