using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisUI.Controls.GameTileView;
using SpectabisUI.Interfaces;
using SpectabisUI.ViewModels;

namespace SpectabisUI.Factories
{
    public class GameTileFactory
    {
        private readonly IConfigurationManager _configuration;
        private readonly IBitmapLoader _bitmapLoader;

        public GameTileFactory(IConfigurationManager configurationLoader, IBitmapLoader bitmapLoader)
        {
            _configuration = configurationLoader;
            _bitmapLoader = bitmapLoader;
        }

        public GameTileView Create(GameProfile game)
        {
            var sizeModifier = _configuration.UserInterface.BoxArtSizeModifier;
            var tileGapSize = _configuration.UserInterface.BoxArtGapSize;
            var boxartBitmap = _bitmapLoader.GetBoxArt(game);

            var viewModel = new GameTileViewModel(game)
            {
                Boxart = boxartBitmap,
            };

            var tileView = new GameTileView(game, viewModel)
            {
                Height = _configuration.UserInterface.BoxArtHeight * sizeModifier,
                Width = _configuration.UserInterface.BoxArtWidth * sizeModifier,
                Margin = new Avalonia.Thickness(0, 0, tileGapSize, tileGapSize),
            };

            if (boxartBitmap != null)
            {
                tileView.LoadBoxart(boxartBitmap);
            }

            return tileView;
        }
    }
}