using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisNext.ViewModels;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class CreateProfile : UserControl, IPage
    {
        private readonly IPageNavigationProvider _navigation;
        private readonly IProfileFactory _profileFactory;
        private readonly IProfileRepository _gameRepo;
        private readonly IBackgroundQueueService _artService;
        private readonly IConfigurationLoader _configuration;
        private readonly IFileBrowserFactory _fileBrowser;

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

        private Button SelectGameButton;
        private Button AddGameButton;

        private readonly CreateProfileViewModel _viewModel;
        private GameProfile _currentProfile;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(CreateProfileViewModel viewModel, IPageNavigationProvider navigation, IProfileFactory profileFactory, IProfileRepository gameRepo, IBackgroundQueueService artService, IConfigurationLoader configuration, IFileBrowserFactory fileBrowser)
        {
            _gameRepo = gameRepo;
            _navigation = navigation;
            _profileFactory = profileFactory;
            _viewModel = viewModel;
            _artService = artService;
            _configuration = configuration;
            _fileBrowser = fileBrowser;

            InitializeComponent();
            RegisterChildren();

            _navigation.PageNavigationEvent += OnNavigation;
            SelectGameButton.Click += SelectGameButtonClick;
            AddGameButton.Click += AddGameButtonClick;
        }

        private void AddGameButtonClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(AddGame);
        }

        private void SelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(SelectGame);
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if (e.Page != this)
            {
                _navigation.PageNavigationEvent -= OnNavigation;
            }
        }

        private void RegisterChildren()
        {
            SelectGameButton = this.FindControl<Button>(nameof(SelectGameButton));
            AddGameButton = this.FindControl<Button>(nameof(AddGameButton));
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        private async Task AddGame()
        {
            if (_currentProfile == null)
            {
                return;
            }

            _currentProfile.Title = _viewModel.GameTitle;
            await _gameRepo.UpsertProfile(_currentProfile).ConfigureAwait(false);

            _artService.QueueForBoxArt(_currentProfile);
            _artService.StartProcessing();

            _navigation.Navigate<GameLibrary>();
        }

        private async Task SelectGame()
        {
            var lastLocation = _configuration.Directories.LastFileBrowserDirectory.LocalPath;
            var filePath = await _fileBrowser.BeginGetSingleFilePath("Select Game File", lastLocation).ConfigureAwait(false);

            if(string.IsNullOrEmpty(filePath))
            {
                return;
            }

            _configuration.Directories.LastFileBrowserDirectory = GetFileDirectory(filePath);
            var updateConfig = _configuration.WriteConfiguration(_configuration.Directories);

            _currentProfile = await _profileFactory.CreateFromPath(filePath).ConfigureAwait(false);

            _viewModel.GameTitle = _currentProfile.Title;
            _viewModel.SerialNumber = _currentProfile.SerialNumber;
            _viewModel.SerialEnabled = string.IsNullOrEmpty(_currentProfile.SerialNumber);

            await updateConfig.ConfigureAwait(true);
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }

        private Uri GetFileDirectory(string filePath)
        {
            var sb = new StringBuilder(filePath);
            var indexOfLast = filePath.LastIndexOf(Path.DirectorySeparatorChar);
            sb.Remove(indexOfLast, sb.Length - indexOfLast);
            return new Uri(sb.ToString(), UriKind.Absolute);
        }
    }
}