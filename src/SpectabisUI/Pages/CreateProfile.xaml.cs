using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Common.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisUI.ViewModels;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Pages
{
    public class CreateProfile : UserControl, IPage
    {
        private readonly IPageNavigationProvider _navigation;
        private readonly IProfileFactory _profileFactory;
        private readonly IProfileRepository _gameRepo;
        private readonly IArtServiceQueue _artService;
        private readonly IConfigurationManager _configuration;
        private readonly IFileBrowserFactory _fileBrowser;
        private readonly IGameDatabaseProvider _gameDb;

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

        private Button SelectGameButton;
        private Button AddGameButton;
        private TextBox TitleTextBox;
        private ListBox TitleSuggestionsBox;

        private readonly CreateProfileViewModel _viewModel;
        private GameProfile _currentProfile;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(CreateProfileViewModel viewModel, IPageNavigationProvider navigation, IProfileFactory profileFactory, IProfileRepository gameRepo, IArtServiceQueue artService, IConfigurationManager configuration, IFileBrowserFactory fileBrowser, IGameDatabaseProvider gameDb)
        {
            _gameRepo = gameRepo;
            _navigation = navigation;
            _profileFactory = profileFactory;
            _viewModel = viewModel;
            _artService = artService;
            _configuration = configuration;
            _fileBrowser = fileBrowser;
            _gameDb = gameDb;

            InitializeComponent();
            RegisterChildren();

            SelectGameButton.Click += SelectGameButtonClick;
            AddGameButton.Click += AddGameButtonClick;
            TitleSuggestionsBox.SelectionChanged += OnTitleSuggestionSelect;

            _navigation.PageNavigationEvent += OnNavigation;
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private async Task SelectGame()
        {
            var lastLocation = _configuration.Directories.LastFileBrowserDirectory.LocalPath;
            var filePath = await _fileBrowser.BeginGetSingleFilePath("Select Game File", lastLocation).ConfigureAwait(false);

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            _configuration.Directories.LastFileBrowserDirectory = GetFileDirectory(filePath);
            var updateConfig = _configuration.WriteConfiguration(_configuration.Directories);

            _currentProfile = await _profileFactory.CreateFromPath(filePath).ConfigureAwait(false);

            _viewModel.GameTitle = _currentProfile.Title;
            _viewModel.SerialNumber = _currentProfile.SerialNumber;
            _viewModel.SerialEnabled = string.IsNullOrEmpty(_currentProfile.SerialNumber);
            _viewModel.FilePath = _currentProfile.FilePath;

            await updateConfig.ConfigureAwait(true);
        }

        private async Task AddGame()
        {
            if (_currentProfile == null)
            {
                return;
            }

            _currentProfile.Title = _viewModel.GameTitle;
            _currentProfile.SerialNumber = _viewModel.SerialNumber;
            await _gameRepo.UpsertProfile(_currentProfile).ConfigureAwait(false);

            _artService.QueueForBoxArt(_currentProfile);
            _artService.StartProcessing();

            _navigation.Navigate<GameLibrary>();
        }

        private void SetTitleSuggestions(IEnumerable<GameMetadata> data)
        {
            if (data?.Any() == true)
            {
                TitleSuggestionsBox.Items = data.Select(x => x.Title);
                TitleSuggestionsBox.IsVisible = true;
                _viewModel.SerialEnabled = true;
            }
            else
            {
                TitleSuggestionsBox.IsVisible = false;
                TitleSuggestionsBox.Items = new List<object>(0);
                _viewModel.SerialEnabled = string.IsNullOrEmpty(_viewModel.GameTitle);
            }
        }

        private Uri GetFileDirectory(string filePath)
        {
            var sb = new StringBuilder(filePath);
            var indexOfLast = filePath.LastIndexOf(Path.DirectorySeparatorChar);
            sb.Remove(indexOfLast, sb.Length - indexOfLast);
            return new Uri(sb.ToString(), UriKind.Absolute);
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if (e.Page != this)
            {
                _navigation.PageNavigationEvent -= OnNavigation;
            }
        }

        private async void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewModel.GameTitle))
            {
                await OnGameTitleFieldChange().ConfigureAwait(false);
            }
        }

        private async Task OnGameTitleFieldChange()
        {
            if (_viewModel.GameTitle.Length < 3)
            {
                Dispatcher.UIThread.Post(() => SetTitleSuggestions(null));
                return;
            }

            try
            {
                var query = await _gameDb.QueryByTitle(_viewModel.GameTitle).ConfigureAwait(true);
                Dispatcher.UIThread.Post(() => SetTitleSuggestions(query));
            }
            catch (Exception ex)
            {
                Logging.WriteLine($"Failed to query database by title `{_viewModel.GameTitle}`");
                Logging.WriteLine(ex.Message);
            }
        }

        private void AddGameButtonClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(AddGame);
        }

        private void SelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(SelectGame);
        }

        private async void OnTitleSuggestionSelect(object sender, SelectionChangedEventArgs e)
        {
            foreach (string item in e.AddedItems)
            {
                var game = await _gameDb.GetNearestByTitle(item).ConfigureAwait(false);
                _viewModel.GameTitle = game.Title;
                _viewModel.SerialNumber = game.Serial;
                _viewModel.SerialEnabled = false;
                Dispatcher.UIThread.Post(() => SetTitleSuggestions(null));
            }
        }

        private void RegisterChildren()
        {
            SelectGameButton = this.FindControl<Button>(nameof(SelectGameButton));
            AddGameButton = this.FindControl<Button>(nameof(AddGameButton));
            TitleTextBox = this.FindControl<TextBox>(nameof(TitleTextBox));
            TitleSuggestionsBox = this.FindControl<ListBox>(nameof(TitleSuggestionsBox));
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        ~CreateProfile()
        {
            Logging.WriteLine("Destroying CreateProfile page");
        }
    }
}