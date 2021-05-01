using System;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
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
        private readonly IProfileRepository _profileRepo;

        [Obsolete("XAMLIL placeholder", true)]
        public GameSettings()
        {
        }

        public GameSettings(IGameConfigurationService gameConfig, GameSettingsViewModel viewModel, IProfileRepository profileRepo)
        {
            _gameConfig = gameConfig;
            _viewModel = viewModel;
            _profileRepo = profileRepo;

            InitializeComponent();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        public void InitializeProfile(GameProfile profile)
        {
            _viewModel.PropertyChanged -= OnViewModelUpdated;

            _viewModel.Id = profile.Id;
            _viewModel.Title = profile.Title;
            _viewModel.Fullscreen = (profile.LaunchOptions & EmulatorLaunchOptions.Fullscreen) != 0;

            _viewModel.PropertyChanged += OnViewModelUpdated;
        }

        private async void OnViewModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            var profile = _profileRepo.Get(_viewModel.Id);

            if(_viewModel.Fullscreen)
            {
                profile.LaunchOptions &= ~EmulatorLaunchOptions.Windowed;
                profile.LaunchOptions |= EmulatorLaunchOptions.Fullscreen;
            }
            else
            {
                profile.LaunchOptions &= ~EmulatorLaunchOptions.Fullscreen;
                profile.LaunchOptions |= EmulatorLaunchOptions.Windowed;
            }

            await _profileRepo.UpsertProfile(profile).ConfigureAwait(false);
        }
    }
}