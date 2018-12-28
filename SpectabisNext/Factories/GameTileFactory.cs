using SpectabisLib.Models;
using SpectabisNext.Controls.GameTile;
using SpectabisNext.Models.Configuration;

namespace SpectabisNext.Factories
{
    public class GameTileFactory
    {
        private readonly UIConfiguration _uiConfiguration;
        public GameTileFactory(UIConfiguration uiConfiguration)
        {
            _uiConfiguration = uiConfiguration;
        }

        public GameTileView Create(GameProfile game)
        {
            var sizeModifier = _uiConfiguration.BoxArtSizeModifier;
            var tileGapSize = _uiConfiguration.BoxArtGapSize;

                var tileView = new GameTileView(game)
                {
                    Height = _uiConfiguration.BoxArtHeight * sizeModifier,
                    Width = _uiConfiguration.BoxArtWidth * sizeModifier,
                    Margin = new Avalonia.Thickness(0, 0, tileGapSize, tileGapSize)
                };

            return tileView;
        }
    }
}