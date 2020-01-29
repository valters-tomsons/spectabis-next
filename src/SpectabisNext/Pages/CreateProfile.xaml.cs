using System;
using System.Linq;
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

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

        private Button SelectGameButton;
        private readonly CreateProfileViewModel _viewModel;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(CreateProfileViewModel viewModel,IPageNavigationProvider navigation, IBitmapLoader bitmapLoader, IProfileFactory profileFactory)
        {
            _navigation = navigation;
            _bitmapLoader = bitmapLoader;
            _profileFactory = profileFactory;
            _viewModel = viewModel;

            InitializeComponent();
            RegisterChildren();

            _navigation.PageNavigationEvent += OnNavigation;
            SelectGameButton.Click += SelectGameButtonClick;
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
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        private async void SelectGame()
        {
            var dialogWindow = new Window();
            var fileDialog = new OpenFileDialog();

            var fileResult = await fileDialog.ShowAsync(dialogWindow).ConfigureAwait(false);

            if(fileResult == null)
            {
                return;
            }

            var filePath = string.Concat(fileResult);

            var profile = await _profileFactory.CreateFromPath(filePath).ConfigureAwait(false);

            _viewModel.SerialNumber = profile.SerialNumber;

            Console.WriteLine(profile.SerialNumber);
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }
    }
}