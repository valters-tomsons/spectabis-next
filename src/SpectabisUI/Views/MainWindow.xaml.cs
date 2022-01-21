using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Common.Helpers;
using SpectabisUI.Controls.PageIcon;
using SpectabisUI.Pages;
using SpectabisUI.Interfaces;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using ServiceLib.Interfaces;
using System.Diagnostics;
using SpectabisUISpectabisUI.ViewModels;

namespace SpectabisUI.Views
{
    public class MainWindow : Window
    {
        private readonly IConfigurationManager _configuration;
        private readonly IPageNavigationProvider _navigationProvider;
        private readonly IGameLauncher _gameLauncher;
        private readonly ITelemetry _telemetry;
        private readonly IFirstTimeWizardService _firstTimeWizard;
        private readonly IFileBrowserFactory _fileBrowser;
        private readonly MainWindowViewModel _viewModel;

        private Rectangle Titlebar;
        private StackPanel TitlebarPanel;
        private ContentControl ContentContainer;

        [Obsolete("XAMLIL placeholder", true)]
        public MainWindow()
		{
		}

        public MainWindow(IConfigurationManager configurationLoader, IPageNavigationProvider navigationProvider, IGameLauncher gameLauncher, ITelemetry telemetry, IFirstTimeWizardService firstTimeWizard, IFileBrowserFactory fileBrowser, MainWindowViewModel viewModel)
		{
            _configuration = configurationLoader;
            _navigationProvider = navigationProvider;
            _gameLauncher = gameLauncher;
            _telemetry = telemetry;
            _firstTimeWizard = firstTimeWizard;
            _fileBrowser = fileBrowser;

			_viewModel = viewModel;
            DataContext = _viewModel;

            Closed += OnWindowClosed;

            if(configurationLoader.Spectabis.EnableTelemetry)
            {
                _telemetry.InitializeTelemetry();
            }

            InitializeSpectabis();
            SetInitialPage();
		}

        private void InitializeSpectabis()
        {
            InitializeFileSystem.Initialize();

            InitializeComponent();
            RegisterChildren();

            // Ugly hacks and workarounds go here
            _navigationProvider.Internals_ReferenceContainer(ContentContainer);
            _navigationProvider.Internals_ReferenceNavigationControls(TitlebarPanel, OnIconPress);
            _fileBrowser.Internals_SetRootWindow(this);
            // ---

            FillElementColors();

            ContentContainer.PropertyChanged += OnContentContainerPropertyChanged;
            _navigationProvider.GeneratePageIcons();
        }

        [Conditional("DEBUG")]
        private void AttachDevTools()
        {
            Logging.WriteLine("Attaching Avalonia dev tools");
            AttachDevTools();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _gameLauncher.StopGame();
        }

        private void OnIconPress(object sender, EventArgs e)
        {
            var icon = (PageIcon)sender;
            var page = icon.Destination;

            _navigationProvider.NavigatePage(page);
        }

        private void FillElementColors()
        {
            Titlebar.Fill = _configuration.UserInterface.TitlebarGradient;
        }

        private void RegisterChildren()
        {
            Titlebar = this.FindControl<Rectangle>(nameof(Titlebar));
            TitlebarPanel = this.FindControl<StackPanel>(nameof(TitlebarPanel));
            ContentContainer = this.FindControl<ContentControl>(nameof(ContentContainer));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnContentContainerPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Content")
            {
                NavigationBarVisiblity((IPage)e.NewValue);
            }
        }

        private void NavigationBarVisiblity(IPage newContent)
        {
            this.FindControl<Grid>("TitlebarContainer").IsVisible = !newContent.HideTitlebar;
        }

        private void SetInitialPage()
        {
            var initialWizard = _firstTimeWizard.IsRequired();

            if(initialWizard)
            {
                _navigationProvider.Navigate<FirstTimeWizard>();
            }
            else
            {
                _navigationProvider.Navigate<GameLibrary>();
            }
        }
    }
}