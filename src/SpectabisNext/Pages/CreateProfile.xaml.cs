using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Repositories;
using SpectabisLib.Services;
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

        private Image BoxArtImage;
        private Button SelectGameButton;

        private GameProfile _currentProfile { get; set; }

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(IPageNavigationProvider navigation, IBitmapLoader bitmapLoader, IProfileFactory profileFactory)
        {
            InitializeComponent();
            RegisterChildren();

            _navigation = navigation;
            _bitmapLoader = bitmapLoader;
            _profileFactory = profileFactory;

            _navigation.PageNavigationEvent += OnNavigation;
            SelectGameButton.Click += SelectGameButtonClick;

            SetupLayout();
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
                return;
            }
        }

        private void RegisterChildren()
        {
            BoxArtImage = this.FindControl<Image>(nameof(BoxArtImage));
            SelectGameButton = this.FindControl<Button>(nameof(SelectGameButton));
        }

        private void SetupLayout()
        {
            BoxArtImage.Source = _bitmapLoader.DefaultBoxart;
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void SelectGame()
        {
            var dialogWindow = new Window();
            var fileDialog = new OpenFileDialog();

            var fileResult = await fileDialog.ShowAsync(dialogWindow);
            var filePath = string.Concat(fileResult);

            var profile = _profileFactory.CreateFromPath(filePath);
            _currentProfile = profile;
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }
    }
}