using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SpectabisNext.Controls.PageIcon;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisUI.Controls;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly PageRepository _pageRepository;
        private readonly ConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _navigationProvider;
        private Rectangle Titlebar;
        private StackPanel TitlebarPanel;
        private ContentControl ContentContainer;

        public MainWindow(PageRepository pageRepository, ConfigurationLoader configurationLoader, IPageNavigationProvider navigationProvider)
        {
            _configuration = configurationLoader;
            _navigationProvider = navigationProvider;
            _pageRepository = pageRepository;

            InitializeComponent();
            RegisterChildern();
            FillElementColors();
            GeneratePageIcons();

            ContentContainer.PropertyChanged += OnContentContainerPropertyChanged;

            SetInitialPage();
        }

        private void GeneratePageIcons()
        {
            var allPages = _pageRepository.All.Where(x => x.ShowInTitlebar);

            foreach (var page in allPages)
            {
                var pageIcon = new PageIcon(page);
                pageIcon.InvokedCallback += OnIconPress;
                TitlebarPanel.Children.Add(pageIcon);
            }
        }

        private void OnIconPress(object sender, EventArgs e)
        {
            var icon = (PageIcon) sender;
            ContentContainer.Content = icon.Destination;
        }

        private void FillElementColors()
        {
            Background = _configuration.UserInterface.UIBackgroundGradient;
            Titlebar.Fill = _configuration.UserInterface.TitlebarGradient;
        }

        private void RegisterChildern()
        {
            Titlebar = this.FindControl<Rectangle>("Titlebar");
            TitlebarPanel = this.FindControl<StackPanel>("TitlebarPanel");
            ContentContainer = this.FindControl<ContentControl>("ContentContainer");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnContentContainerPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Content")
            {
                var newContent = (Page) e.NewValue;

                if (newContent.HideTitlebar)
                {
                    this.FindControl<Grid>("TitlebarContainer").IsVisible = false;
                }
                else
                {
                    this.FindControl<Grid>("TitlebarContainer").IsVisible = true;
                }
            }
        }

        private void SetInitialPage()
        {
            _navigationProvider.Navigate<FirstTimeWizard>();
        }

    }
}