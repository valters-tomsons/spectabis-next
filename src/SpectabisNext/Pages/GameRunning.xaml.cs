using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class GameRunning : UserControl, Page
    {
        public string PageTitle { get; } = "PCSX2";
        public bool ShowInTitlebar { get; } = false;
        public bool HideTitlebar { get; } = true;
        public bool ReloadOnNavigation { get; } = true;

        private Button StopGame;

        private readonly IGameLauncher _gameLauncher;
        private readonly IPageNavigationProvider _pageNavigation;

        [Obsolete("XAMLIL placeholder", true)]
        public GameRunning() { }

        public GameRunning(IGameLauncher gameLauncher, IPageNavigationProvider pageNavigation)
        {
            _gameLauncher = gameLauncher;
            _pageNavigation = pageNavigation;

            InitializeComponent();
            RegisterChildren();

            var gameProc = _gameLauncher.GetRunningGame();
            Console.WriteLine($"[GameRunning] Game running:{gameProc.Game.Title} with processId '{gameProc.Process.Id}'");
        }

        private void RegisterChildren()
        {
            StopGame = this.FindControl<Button>("StopGame_Button");
            StopGame.Click += StopGame_Click;

            _gameLauncher.GetRunningGame().GameStopped += OnGameStopped;
        }

        private void OnGameStopped(object sender, EventArgs args)
        {
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