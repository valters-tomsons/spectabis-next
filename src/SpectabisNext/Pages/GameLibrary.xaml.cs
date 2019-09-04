using System;
using Avalonia.Controls;
using Avalonia.Input;
using SpectabisLib.Interfaces;
using SpectabisLib.Repositories;
using SpectabisNext.Controls.GameTileView;
using SpectabisNext.Factories;
using SpectabisUI.Controls;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameLibrary : Page
    {
        private readonly GameProfileRepository _gameRepo;
        private readonly GameTileFactory _tileFactory;
        private readonly IGameLauncher _gameLauncher;
        private readonly IPageNavigationProvider _navigationProvider;

        public GameLibrary(GameProfileRepository gameRepo, GameTileFactory tileFactory, IGameLauncher gameLauncher, IPageNavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _gameLauncher = gameLauncher;
            _gameRepo = gameRepo;

            PageTitle = "Library";
            ShowInTitlebar = true;

            Populate();
        }

        private void Populate()
        {
            var gamePanel = this.FindControl<WrapPanel>("GamePanel");

            foreach (var gameProfile in _gameRepo.GetAll())
            {
                var gameTile = _tileFactory.Create(gameProfile);
                gameTile.PointerPressed += OnGameTileClick;
                gamePanel.Children.Add(gameTile);
            }
        }

        private void OnGameTileClick(object sender, PointerPressedEventArgs e)
        {
            var clickedTile = (GameTileView) sender;
            System.Console.WriteLine($"Launching {clickedTile.Profile.Title}");
            _gameLauncher.Launch(clickedTile.Profile);
            // _navigationProvider.Navigate<GameRunning>();
        }
    }
}