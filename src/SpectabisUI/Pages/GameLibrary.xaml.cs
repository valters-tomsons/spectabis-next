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
using SpectabisUI.Controls.GameTileView;
using SpectabisUI.Factories;
using SpectabisUI.Enums;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;
using SpectabisLib.Interfaces.Controllers;

namespace SpectabisUI.Pages
{
    public class GameLibrary : UserControl, IPage
    {
        public string PageTitle => "Library";
        public bool ShowInTitlebar => true;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => false;

        private readonly IGameLibraryController _libraryController;

        private readonly GameTileFactory _tileFactory;
        private readonly IPageNavigationProvider _navigationProvider;
        private readonly IContextMenuEnumMapper _menuMapper;
        private readonly IArtServiceQueue _queueService;
        private readonly IBitmapLoader _bitmapLoader;
        private readonly IGifProvider _gifProvider;
        private readonly IDirectoryScan _dirScan;

        // TODO: Replace profile management to viewmodel
        private readonly List<GameProfile> LoadedProfiles = new List<GameProfile>();
        private WrapPanel GamePanel;

        [Obsolete("XAMLIL placeholder", true)]
        public GameLibrary()
        {
        }

        public GameLibrary(GameTileFactory tileFactory, IPageNavigationProvider navigationProvider, IContextMenuEnumMapper menuMapper, IArtServiceQueue queueService, IBitmapLoader bitmapLoader, IGifProvider gifProvider, IDirectoryScan dirScan, IGameLibraryController libraryController)
        {
            _libraryController = libraryController;
            _navigationProvider = navigationProvider;
            _tileFactory = tileFactory;
            _queueService = queueService;
            _bitmapLoader = bitmapLoader;
            _gifProvider = gifProvider;
            _dirScan = dirScan;
            _menuMapper = menuMapper;

            _navigationProvider.PageNavigationEvent += OnNavigation;
            _queueService.ItemFinished += OnGameArtDownloaded;

            InitializeComponent();
            RegisterChildren();

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

        // TODO: Replace profile management to viewmodel
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

        // TODO: Replace profile management to viewmodel
        private async Task AddNewGames()
        {
            var newGames = await _libraryController.GetNewGames(LoadedProfiles).ConfigureAwait(true);

            foreach (var item in newGames)
            {
                var gameTile = _tileFactory.Create(item);
                var tile = await Dispatcher.UIThread.InvokeAsync(() => AddProfileTile(gameTile)).ConfigureAwait(true);
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

        // TODO: Replace profile management to viewmodel
        private GameTileView AddProfileTile(GameTileView gameTile)
        {
            gameTile.PointerReleased += OnGameTileClick;

            GamePanel.Children.Add(gameTile);
            LoadedProfiles.Add(gameTile.Profile);

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
            _libraryController.LaunchConfiguration(gameTile.Profile);
            _navigationProvider.Navigate<GameRunning>();
        }

        private void LaunchTile(GameTileView gameTile)
        {
            _libraryController.LaunchGame(gameTile.Profile);
            _navigationProvider.Navigate<GameRunning>();
        }

        private void RemoveGame(GameTileView gameTile)
        {
            _libraryController.DeleteGame(gameTile.Profile);

            LoadedProfiles.Remove(gameTile.Profile);
            Dispatcher.UIThread.Post(() => GamePanel.Children.Remove(gameTile));
        }

        private void OpenWikiPage(GameTileView gameTile)
        {
            _libraryController.OpenWikiPage(gameTile.Profile);
        }
    }
}