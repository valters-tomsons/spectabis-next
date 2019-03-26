using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SpectabisNext.Controls;
using SpectabisNext.Interfaces;
using SpectabisNext.Configuration;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisNext.Controls.PageIcon;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly PageRepository _pageRepository;
        private readonly ConfigurationLoader _configuration;
        private Rectangle Titlebar;
        private StackPanel TitlebarPanel;
        private ContentControl ContentContainer;

        public MainWindow(PageRepository pageRepository, ConfigurationLoader configurationLoader)
        {
            _configuration = configurationLoader;
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
            var page = _pageRepository.GetPage<Settings>();
            var icon = new PageIcon(page);
            icon.InvokedCallback += OnIconPress;
            TitlebarPanel.Children.Add(icon);
        }

        private void OnIconPress(object sender, EventArgs e)
        {
            var icon = (PageIcon) sender;
            System.Console.WriteLine($"Clicked on {icon.Destination.PageTitle}");
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
            ContentContainer.Content = _pageRepository.GetPage<GameLibrary>();
        }
    }
}