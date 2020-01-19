using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces;
using SpectabisLib.Repositories;
using SpectabisNext.Controls.GameTileView;
using SpectabisNext.Factories;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameLibrary : UserControl, Page
    {
        public string PageTitle { get; } = "Library";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = false;

        private readonly GameProfileRepository _gameRepo;
        private readonly GameTileFactory _tileFactory;
        private readonly IGameLauncher _gameLauncher;
        private readonly IPageNavigationProvider _navigationProvider;

        [Obsolete("XAMLIL placeholder", true)]
        public GameLibrary() { }

        public GameLibrary(GameProfileRepository gameRepo, GameTileFactory tileFactory, IGameLauncher gameLauncher, IPageNavigationProvider navigationProvider)
        {

            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _gameLauncher = gameLauncher;
            _gameRepo = gameRepo;

            InitializeComponent();
            Populate();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
            var clickedTile = (GameTileView)sender;

            if (e.MouseButton == MouseButton.Left)
            {
                LaunchTile(clickedTile);
            }
        }

        private void LaunchTile(GameTileView gameTile)
        {
            System.Console.WriteLine($"Launching {gameTile.Profile.Title}");
            _gameLauncher.Launch(gameTile.Profile);
            _navigationProvider.Navigate<GameRunning>();
        }
    }
}