using SpectabisLib.Models;
using SpectabisNext.Controls.GameTile;
using SpectabisNext.Configuration;
using SpectabisNext.Services;

namespace SpectabisNext.Factories
{
    public class GameTileFactory
    {
        private readonly ConfigurationLoader _configuration;

        public GameTileFactory(ConfigurationLoader configurationLoader)
        {
            _configuration = configurationLoader;
        }

        public GameTileView Create(GameProfile game)
        {
            var sizeModifier = _configuration.UserInterface.BoxArtSizeModifier;
            var tileGapSize = _configuration.UserInterface.BoxArtGapSize;

                var tileView = new GameTileView(game)
                {
                    Height = _configuration.UserInterface.BoxArtHeight * sizeModifier,
                    Width = _configuration.UserInterface.BoxArtWidth * sizeModifier,
                    Margin = new Avalonia.Thickness(0, 0, tileGapSize, tileGapSize)
                };

            return tileView;
        }
    }
}