using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Repositories;
using SpectabisLib.Services;
using SpectabisNext.ViewModels;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class CreateProfile : UserControl, IPage
    {
        private readonly IPageNavigationProvider _navigation;
        private readonly IBitmapLoader _bitmapLoader;
        private readonly IProfileFactory _profileFactory;
        private readonly IProfileRepository _gameRepo;

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

        public CreateProfile(CreateProfileViewModel viewModel, IPageNavigationProvider navigation, IBitmapLoader bitmapLoader, IProfileFactory profileFactory, IProfileRepository gameRepo)
        {
            _gameRepo = gameRepo;
            _navigation = navigation;
            _bitmapLoader = bitmapLoader;
            _profileFactory = profileFactory;
            _viewModel = viewModel;

            InitializeComponent();
            RegisterChildren();

            _navigation.PageNavigationEvent += OnNavigation;
            SelectGameButton.Click += SelectGameButtonClick;
            AddGameButton.Click += AddGameButtonClick;
        }

        private void AddGameButtonClick(object sender, RoutedEventArgs e)
        {
            AddGame();
        }

        private void SelectGameButtonClick(object sender, RoutedEventArgs e)
        {
            SelectGame();
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
            _currentProfile.Title = _viewModel.GameTitle;
            await _gameRepo.UpsertProfile(_currentProfile).ConfigureAwait(false);

            _navigation.Navigate<GameLibrary>();
        }

        private async Task SelectGame()
        {
            var dialogWindow = new Window();
            var fileDialog = new OpenFileDialog();

            var fileResult = await fileDialog.ShowAsync(dialogWindow).ConfigureAwait(false);

            if (fileResult == null)
            {
                return;
            }

            var filePath = string.Concat(fileResult);

            _currentProfile = await _profileFactory.CreateFromPath(filePath).ConfigureAwait(false);

            _viewModel.SerialNumber = _currentProfile.SerialNumber;
            _viewModel.GameTitle = _currentProfile.Title;

            Console.WriteLine(_currentProfile.SerialNumber);
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }
    }
}