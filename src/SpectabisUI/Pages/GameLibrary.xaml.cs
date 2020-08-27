using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Services;
using SpectabisUI.Controls.GameTileView;
using SpectabisUI.Factories;
using SpectabisUI.Enums;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Pages
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
        private readonly IContextMenuEnumMapper _menuMapper;
        private readonly IDiscordService _discordService;
        private readonly IArtServiceQueue _queueService;
        private readonly IBitmapLoader _bitmapLoader;
        private readonly IGifProvider _gifProvider;
        private readonly IDirectoryScan _dirScan;

        private readonly List<GameProfile> LoadedProfiles = new List<GameProfile>();

        private WrapPanel GamePanel;

        [Obsolete("XAMLIL placeholder", true)]
        public GameLibrary() { }

        public GameLibrary(IProfileRepository gameRepo, GameTileFactory tileFactory, IGameLauncher gameLauncher, IPageNavigationProvider navigationProvider, IContextMenuEnumMapper menuMapper, IDiscordService discordService, IArtServiceQueue queueService, IBitmapLoader bitmapLoader, IGifProvider gifProvider, IDirectoryScan dirScan)
        {
            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _gameLauncher = gameLauncher;
            _gameRepo = gameRepo;
            _queueService = queueService;
            _bitmapLoader = bitmapLoader;
            _gifProvider = gifProvider;
            _dirScan = dirScan;

            _navigationProvider.PageNavigationEvent += OnNavigation;

            InitializeComponent();
            RegisterChildren();

            Dispatcher.UIThread.Post(Populate);
            _menuMapper = menuMapper;
            _discordService = discordService;

            _queueService.ItemFinished += OnGameArtDownloaded;

            _dirScan.ScanNewGames();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnGameArtDownloaded(object sender, EventArgs e)
        {
            var game = _queueService.PopFinishedGames();
            Dispatcher.UIThread.Post(() => RefreshGameTileArt(game));
            _gifProvider.DisponseSpinner(game);
        }

        private GameTileView GetGameTileControl(GameProfile game)
        {
            var tileControls = GamePanel.Children;

            foreach (var item in tileControls)
            {
                if (item.GetType() != typeof(GameTileView))
                {
                    continue;
                }

                var tile = (GameTileView) item;

                if (tile.Profile != game)
                {
                    continue;
                }

                return item as GameTileView;
            }

            return null;
        }

        private void RefreshGameTileArt(GameProfile game)
        {
            var tile = GetGameTileControl(game);
            var bitmap = _bitmapLoader.GetBoxArt(game);
            tile.LoadBoxart(bitmap);
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if (e.Page == this)
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
                var tile = await Dispatcher.UIThread.InvokeAsync(() => AddProfileTile(item)).ConfigureAwait(true);
                var isQueued = _queueService.IsProcessing(item);

                if(isQueued)
                {
                    Dispatcher.UIThread.Post(() => _gifProvider.StartSpinner(item, tile.BoxArt));
                }
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

        private GameTileView AddProfileTile(GameProfile gameProfile)
        {
            var gameTile = _tileFactory.Create(gameProfile);
            gameTile.PointerReleased += OnGameTileClick;

            GamePanel.Children.Add(gameTile);
            LoadedProfiles.Add(gameProfile);

            return gameTile;
        }

        private void OnGameContextMenuClick(object sender, PointerReleasedEventArgs e)
        {
            var obj = (ContextMenu) sender;
            var tile = (GameTileView) obj.Parent.Parent;

            // TODO: Please fix this mess someday
            var selectd = (GameContextMenuItem) obj.SelectedIndex;

            if (selectd == GameContextMenuItem.Launch)
            {
                LaunchTile(tile);
            }

            if (selectd == GameContextMenuItem.Configure)
            {
                LaunchConfiguration(tile);
            }

            if (selectd == GameContextMenuItem.Remove)
            {
                RemoveGame(tile);
            }

            if (selectd == GameContextMenuItem.OpenWiki)
            {
                OpenWikiPage(tile);
            }

            // TODO: Should share one global context menu when Avalonia supports it
            obj.Close();
            obj.PointerReleased -= OnGameContextMenuClick;
        }

        private void OnGameTileClick(object sender, PointerReleasedEventArgs e)
        {
            var clickedTile = (GameTileView) sender;
            var pointerUpdate = e.GetCurrentPoint(null).Properties.PointerUpdateKind;

            if (pointerUpdate == PointerUpdateKind.LeftButtonReleased)
            {
                LaunchTile(clickedTile);
            }

            if (pointerUpdate == PointerUpdateKind.RightButtonReleased)
            {
                var menuItems = _menuMapper.GetDisplayNames();

                // TODO: Should share one global context menu when Avalonia supports it
                var contextMenu = new ContextMenu() { Items = menuItems };
                contextMenu.PointerReleased += OnGameContextMenuClick;
                contextMenu.Open(clickedTile);
            }
        }

        private void LaunchConfiguration(GameTileView gameTile)
        {
            Console.WriteLine($"[GameLibrary] Configuring {gameTile.Profile.Title}");

            _gameLauncher.StartConfiguration(gameTile.Profile);
            _navigationProvider.Navigate<GameRunning>();
        }

        private void LaunchTile(GameTileView gameTile)
        {
            Console.WriteLine($"[GameLibrary] Launching {gameTile.Profile.Title}");

            _gameLauncher.StartGame(gameTile.Profile);
            _discordService.SetGamePresence(gameTile.Profile);

            _navigationProvider.Navigate<GameRunning>();
        }

        private void RemoveGame(GameTileView gameTile)
        {
            Console.WriteLine($"[GameLibrary] Removing {gameTile.Profile.Id}");

            _gameRepo.DeleteProfile(gameTile.Profile);
            LoadedProfiles.Remove(gameTile.Profile);

            Dispatcher.UIThread.Post(() => GamePanel.Children.Remove(gameTile));
        }

        private void OpenWikiPage(GameTileView gameTile)
        {
            var titleQuery = new StringBuilder(gameTile.Profile.Title);
            titleQuery.Replace(" - ", ":+");
            titleQuery.Replace(" ", "+");
            titleQuery.Replace("++", ":+");

            var wikiUrl = new Uri($"http://wiki.pcsx2.net/index.php?search={titleQuery}", UriKind.Absolute);
            BrowserProvider.OpenWebBrowser(wikiUrl);
        }
    }
}