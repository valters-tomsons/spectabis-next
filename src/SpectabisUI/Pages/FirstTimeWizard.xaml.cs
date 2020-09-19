using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Interfaces;
using SpectabisUI.ViewModels;
using SpectabisUI.Interfaces;

namespace SpectabisUI.Pages
{
    public class FirstTimeWizard : UserControl, IPage
    {
        public string PageTitle => "Wizard";
        public bool ShowInTitlebar => false;
        public bool HideTitlebar => true;
        public bool ReloadOnNavigation => true;

        private readonly IConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _pageNavigator;
        private readonly IFirstTimeWizardService _wizardService;
        private readonly IFileBrowserFactory _fileBrowser;

        private Button BrowseExecutableButton;
        private Button BrowseConfigurationButton;
        private Button FinishButton;
        private Button TelemetrySwitchButton;

        private readonly FirstTimeWizardViewModel _viewModel;

        [Obsolete("XAMLIL placeholder", true)]
        public FirstTimeWizard() { }

        public FirstTimeWizard(IConfigurationLoader configuration, IPageNavigationProvider pageNavigator, IFirstTimeWizardService wizardService, IFileBrowserFactory fileBrowser, FirstTimeWizardViewModel viewModel)
        {
            _configuration = configuration;
            _pageNavigator = pageNavigator;
            _wizardService = wizardService;
            _fileBrowser = fileBrowser;
            _viewModel = viewModel;
            Background = _configuration.UserInterface.TitlebarGradient;

            InitializeComponent();
            RegisterChildren();

            _viewModel.TelemetryMessage = _wizardService.GetTelemetryStatusMessage(false);
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContext = _viewModel;
        }

        private void RegisterChildren()
        {
            BrowseExecutableButton = this.FindControl<Button>(nameof(BrowseExecutableButton));
            BrowseConfigurationButton = this.FindControl<Button>(nameof(BrowseConfigurationButton));
            FinishButton = this.FindControl<Button>(nameof(FinishButton));
            TelemetrySwitchButton = this.FindControl<Button>(nameof(TelemetrySwitchButton));

            BrowseExecutableButton.Click += BrowseExecutableClick;
            FinishButton.Click += FirstButtonClick;
            BrowseConfigurationButton.Click += BrowseConfigClick;
            TelemetrySwitchButton.Click += TelemetrySwitchClick;
        }

        private void TelemetrySwitchClick(object sender, RoutedEventArgs e)
        {
            _viewModel.TelemetryMessage = _wizardService.GetTelemetryStatusMessage(true);
        }

        private void BrowseConfigClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(Open_BrowseConfiguration);
        }

        private async Task Open_BrowseConfiguration()
        {
            var oldPath = _configuration.Directories.PCSX2ConfigurationPath;
            var browserResult = await _fileBrowser.BeginGetDirectoryPath("Select PCSX2 Path", oldPath.LocalPath).ConfigureAwait(true);

            if(string.IsNullOrEmpty(browserResult))
            {
                return;
            }

            _configuration.Directories.PCSX2ConfigurationPath = new Uri(browserResult, UriKind.Absolute);
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);
            _viewModel.ConfigurationPath = browserResult;
        }

        private void BrowseExecutableClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(Open_BrowseExectuable);
        }

        private async Task Open_BrowseExectuable()
        {
            var oldPath = _configuration.Directories.PCSX2Executable;
            var browserResult = await _fileBrowser.BeginGetSingleFilePath("Select PCSX2 Path", oldPath.LocalPath).ConfigureAwait(true);

            if(string.IsNullOrEmpty(browserResult))
            {
                return;
            }

            _configuration.Directories.PCSX2Executable = new Uri(browserResult, UriKind.Absolute);
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);
            _viewModel.ExecutablePath= browserResult;
        }

        private void FirstButtonClick(object sender, RoutedEventArgs e)
        {
            _wizardService.WriteInitialConfigs();
            _pageNavigator.Navigate<GameLibrary>();
            _wizardService.DisableFirstTimeWizard();
        }
    }
}