using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces.Services;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;
using SpectabisUI.ViewModels;

namespace SpectabisUI.Pages
{
    public class GameSettings : UserControl, IPage
    {
        public string PageTitle => "PCSX2";
        public bool ShowInTitlebar => false;
        public bool HideTitlebar => false;
        public bool ReloadOnNavigation => false;

        private readonly IGameConfigurationService _gameConfig;
        private readonly GameSettingsViewModel _viewModel;

        [Obsolete("XAMLIL placeholder", true)]
        public GameSettings()
        {
        }

        public GameSettings(IGameConfigurationService gameConfig, GameSettingsViewModel viewModel)
        {
            _gameConfig = gameConfig;
            _viewModel = viewModel;

            InitializeComponent();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        public void SetCurrentProfile(GameProfile profile)
        {
            _viewModel.Title = profile.Title;
        }
    }
}