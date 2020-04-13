using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Interfaces;
using SpectabisLib.Repositories;
using SpectabisNext.Controls.GameTileView;
using SpectabisNext.Factories;
using SpectabisNext.ViewModels;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameLibrary : UserControl, IPage
    {
        public string PageTitle => "Library";
        public bool ShowInTitlebar => true;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => false;

        private readonly IProfileRepository _gameRepo;
        private readonly GameTileFactory _tileFactory;
        private readonly IGameLauncher _gameLauncher;
        private readonly IPageNavigationProvider _navigationProvider;

        [Obsolete("XAMLIL placeholder", true)]
        public GameLibrary() { }

        public GameLibrary(IProfileRepository gameRepo, GameTileFactory tileFactory, IGameLauncher gameLauncher, IPageNavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _gameLauncher = gameLauncher;
            _gameRepo = gameRepo;

            InitializeComponent();
            Dispatcher.UIThread.Post(Populate);
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void Populate()
        {
            var gamePanel = this.FindControl<WrapPanel>("GamePanel");
            var games = await _gameRepo.GetAll().ConfigureAwait(true);

            foreach (var gameProfile in games)
            {
                var gameTile = _tileFactory.Create(gameProfile);
                gameTile.PointerReleased += OnGameTileClick;
                gamePanel.Children.Add(gameTile);
            }
        }

        private void OnGameTileClick(object sender, PointerReleasedEventArgs e)
        {
            var clickedTile = (GameTileView) sender;
            var pointerUpdate = e.GetCurrentPoint(null).Properties.PointerUpdateKind;

            if (pointerUpdate == PointerUpdateKind.LeftButtonReleased)
            {
                LaunchTile(clickedTile);
            }
        }

        private void LaunchTile(GameTileView gameTile)
        {
            Console.WriteLine($"[GameLibrary] Launching {gameTile.Profile.Title}");

            _gameLauncher.StartGame(gameTile.Profile);
            _navigationProvider.Navigate<GameRunning>();
        }
    }
}