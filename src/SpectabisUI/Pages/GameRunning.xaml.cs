using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Pages
{
    public class GameRunning : UserControl, IPage
    {
        public string PageTitle => "PCSX2";
        public bool ShowInTitlebar => false;
        public bool HideTitlebar => true;
        public bool ReloadOnNavigation => true;

        private Button StopGame;

        private readonly IGameLauncher _gameLauncher;
        private readonly IPageNavigationProvider _pageNavigation;
        private readonly IDiscordService _discordService;

        [Obsolete("XAMLIL placeholder", true)]
        public GameRunning()
        { }

        public GameRunning(IGameLauncher gameLauncher, IPageNavigationProvider pageNavigation, IDiscordService discordService)
        {
            _gameLauncher = gameLauncher;
            _pageNavigation = pageNavigation;
            _discordService = discordService;

            InitializeComponent();
            RegisterChildren();

            var gameProc = _gameLauncher.GetRunningGame();
            Logging.WriteLine($"Game running:{gameProc.Game.Title} with processId '{gameProc.Process.Id}'");
        }

        private void RegisterChildren()
        {
            StopGame = this.FindControl<Button>("StopGame_Button");
            StopGame.Click += StopGame_Click;

            _gameLauncher.GetRunningGame().GameStopped += OnGameStopped;
        }

        private void OnGameStopped(object sender, EventArgs args)
        {
            _discordService.SetMenuPresence();
            _pageNavigation.Navigate<GameLibrary>();
        }

        private void StopGame_Click(object sender, RoutedEventArgs e)
        {
            _gameLauncher.StopGame();
            _pageNavigation.Navigate<GameLibrary>();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}