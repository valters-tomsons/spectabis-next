using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Common.Helpers;
using EmuConfig.Enums;
using SpectabisLib.Enums;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Controllers;
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
        private readonly IGameLibraryController _libraryController;
        private readonly IPageNavigationProvider _navigationProvider;

        private ProfileConfiguration _currentConfig;

        [Obsolete("XAMLIL placeholder", true)]
        public GameSettings()
        {
        }

        public GameSettings(IGameConfigurationService gameConfig, GameSettingsViewModel viewModel, IProfileRepository profileRepo, IGameLibraryController libraryController, IPageNavigationProvider navigationProvider)
        {
            _gameConfig = gameConfig;
            _viewModel = viewModel;
            _profileRepo = profileRepo;
            _libraryController = libraryController;

            _viewModel.LaunchPCSX2.Subscribe(async _ => await LaunchPCSX2_Click().ConfigureAwait(true));

            InitializeComponent();
            _navigationProvider = navigationProvider;
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        public async Task LaunchPCSX2_Click()
        {
            var game = await _profileRepo.Get(_viewModel.Id).ConfigureAwait(true);
            _libraryController.LaunchConfiguration(game);

            _navigationProvider.Navigate<GameRunning>();
        }

        public async Task InitializeProfile(GameProfile profile)
        {
            _viewModel.PropertyChanged -= OnViewModelUpdated;

            try
            {
                _currentConfig = await _gameConfig.Get(profile.Id).ConfigureAwait(false);
                _viewModel.ShowSettings = true;
                _viewModel.Resolution = _currentConfig.GSdxConfig.UpscaleFactor.ToString();
            }
            catch(Exception e)
            {
                _viewModel.ShowSettings = false;

                Logging.WriteLine("Failed to read profile configuration");
                Logging.WriteLine(e.Message);
            }
            finally{
                _viewModel.Id = profile.Id;
                _viewModel.Title = profile.Title;
                _viewModel.Fullscreen = (profile.LaunchOptions & EmulatorLaunchOptions.Fullscreen) != 0;

                _viewModel.PropertyChanged += OnViewModelUpdated;
            }
        }

        private async void OnViewModelUpdated(object sender, PropertyChangedEventArgs e)
        {
            if(!_viewModel.ShowSettings)
            {
                return;
            }

            var profile = await _profileRepo.Get(_viewModel.Id).ConfigureAwait(false);

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

            _currentConfig.GSdxConfig.UpscaleFactor = Enum.Parse<UpscaleFactor>(_viewModel.Resolution, true);

            await _gameConfig.UpdateConfiguration(_viewModel.Id, _currentConfig.GSdxConfig).ConfigureAwait(false);
            await _profileRepo.UpsertProfile(profile).ConfigureAwait(false);
        }
    }
}