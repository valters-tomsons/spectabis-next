using System;
using Avalonia.Controls;
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
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;
        private readonly IGameLauncher _gameLauncher;
        // private 

        [Obsolete("XAMLIL placeholder", true)]
        public GameRunning() { }

        public GameRunning(IGameLauncher gameLauncher)
        {
            _gameLauncher = gameLauncher;
            InitializeComponent();

            Console.WriteLine($"[GameRunning] Game running: {_gameLauncher.GetRunningGame().Game.Title}");
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}