using Avalonia.Controls;
using SpectabisLib.Repositories;
using SpectabisNext.Factories;
using SpectabisUI.Controls;

namespace SpectabisNext.Pages
{
    public class GameLibrary : Page
    {
        private readonly GameProfileRepository _gameRepo;
        private readonly GameTileFactory _tileFactory;
        public GameLibrary(GameProfileRepository gameRepo, GameTileFactory tileFactory)
        {
            _tileFactory = tileFactory;
            _gameRepo = gameRepo;

            PageTitle = "Library";
            ShowInTitlebar = true;

            Populate();
        }

        private void Populate()
        {
            var gamePanel = this.FindControl<WrapPanel>("GamePanel");
        
            foreach(var gameProfile in _gameRepo.GetAll())
            {
                var gameTile = _tileFactory.Create(gameProfile);
                gamePanel.Children.Add(gameTile);
            }
        }
    }
}