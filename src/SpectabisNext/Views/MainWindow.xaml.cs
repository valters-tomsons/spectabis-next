using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SpectabisLib.Helpers;
using SpectabisNext.Controls.PageIcon;
using SpectabisNext.Pages;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Views
{
    public class MainWindow : Window
    {
        private readonly IConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _navigationProvider;
        private Rectangle Titlebar;
        private StackPanel TitlebarPanel;
        private ContentControl ContentContainer;

        [Obsolete("XAMLIL placeholder", true)]
        public MainWindow() { }

        public MainWindow(IConfigurationLoader configurationLoader, IPageNavigationProvider navigationProvider)
        {
            _configuration = configurationLoader;
            _navigationProvider = navigationProvider;

            InitializeFileSystem.Initialize();

            InitializeComponent();
            RegisterChildern();

            _navigationProvider.ReferenceContainer(ContentContainer);
            _navigationProvider.ReferenceNavigationControls(TitlebarPanel, OnIconPress);

            FillElementColors();

            ContentContainer.PropertyChanged += OnContentContainerPropertyChanged;

            SetInitialPage();
            _navigationProvider.GeneratePageIcons();
        }

        private void OnIconPress(object sender, EventArgs e)
        {
            var icon = (PageIcon)sender;
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
                NavigationBarVisiblity((Page)e.NewValue);
            }
        }

        private void NavigationBarVisiblity(Page newContent)
        {
            if (newContent.HideTitlebar)
            {
                this.FindControl<Grid>("TitlebarContainer").IsVisible = false;
            }
            else
            {
                this.FindControl<Grid>("TitlebarContainer").IsVisible = true;
            }
        }

        private void SetInitialPage()
        {
            _navigationProvider.Navigate<FirstTimeWizard>();
            // _navigationProvider.Navigate<FileBrowser>();
        }

    }
}