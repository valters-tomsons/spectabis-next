using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SpectabisLib.Repositories;
using SpectabisUI.Events;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class CreateProfile : UserControl, IPage
    {
        private readonly GameProfileRepository _gameRepo;
        private readonly IPageNavigationProvider _navigation;

        public string PageTitle { get; } = "Add Game";
        public bool ShowInTitlebar { get; } = true;
        public bool HideTitlebar { get; } = false;
        public bool ReloadOnNavigation { get; } = true;

        [Obsolete("XAMLIL placeholder", true)]
        public CreateProfile() { }

        public CreateProfile(GameProfileRepository gameRepo, IPageNavigationProvider navigation)
        {
            _gameRepo = gameRepo;
            _navigation = navigation;

            InitializeComponent();

            _navigation.PageNavigationEvent += OnNavigation;
        }

        private void OnNavigation(object sender, NavigationArgs e)
        {
            if(e.Page != this)
            {
                _navigation.PageNavigationEvent -= OnNavigation;
                return;
            }

            SelectGame();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SelectGame()
        {
            var dialogWindow = new Window();
            var fileDialog = new OpenFileDialog();
            fileDialog.ShowAsync(dialogWindow);
        }

        ~CreateProfile()
        {
            Console.WriteLine("Destroying CreateProfile page");
        }
    }
}