using SpectabisLib.Models;
using SpectabisNext.Controls.GameTile;

namespace SpectabisNext.Factories
{
    public class GameTileFactory
    {
        public GameTileFactory()
        {

        }

        public GameTileView Create(GameProfile game)
        {
            var tileView = new GameTileView(game);
            return tileView;
        }
    }
}