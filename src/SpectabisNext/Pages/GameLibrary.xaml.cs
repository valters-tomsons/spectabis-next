using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Repositories;
using SpectabisNext.Controls.GameTileView;
using SpectabisNext.Factories;
using SpectabisNext.ViewModels;
using SpectabisUI.Events;
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

        private readonly List<GameProfile> LoadedProfiles = new List<GameProfile>();

        private WrapPanel GamePanel;

        [Obsolete("XAMLIL placeholder", true)]
        public GameLibrary() { }

        public GameLibrary(IProfileRepository gameRepo, GameTileFactory tileFactory, IGameLauncher gameLauncher, IPageNavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _gameLauncher = gameLauncher;
            _gameRepo = gameRepo;

            _navigationProvider.PageNavigationEvent += OnNavigation;

            InitializeComponent();
            RegisterChildren();
            Dispatcher.UIThread.Post(Populate);
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if(e.Page == this)
            {
                Dispatcher.UIThread.InvokeAsync(AddNewGames);
            }
        }

        private async Task AddNewGames()
        {
            var allGames = await _gameRepo.GetAll().ConfigureAwait(true);
            var newGames = allGames.Except(LoadedProfiles);

            foreach (var item in newGames)
            {
                await Dispatcher.UIThread.InvokeAsync(() => AddProfileTile(item)).ConfigureAwait(true);
            }
        }

        private void RegisterChildren()
        {
            GamePanel = this.FindControl<WrapPanel>(nameof(GamePanel));
        }

        private async void Populate()
        {
            var games = await _gameRepo.GetAll().ConfigureAwait(true);

            foreach (var gameProfile in games)
            {
                AddProfileTile(gameProfile);
            }
        }

        private void AddProfileTile(GameProfile gameProfile)
        {
            var gameTile = _tileFactory.Create(gameProfile);
            gameTile.PointerReleased += OnGameTileClick;
            GamePanel.Children.Add(gameTile);
            LoadedProfiles.Add(gameProfile);
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