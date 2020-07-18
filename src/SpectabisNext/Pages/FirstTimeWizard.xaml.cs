using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using SpectabisLib.Interfaces;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class FirstTimeWizard : UserControl, IPage
    {
        public string PageTitle => "Wizard";
        public bool ShowInTitlebar => false;
        public bool HideTitlebar => true;
        public bool ReloadOnNavigation => true;

        private readonly IConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _pageNavigator;
        private readonly IFirstTimeWizard _wizardService;
        private readonly IFileBrowserFactory _fileBrowser;
        private Button BrowseExecutableButton;
        private Button BrowseConfigurationButton;
        private Button FinishButton;

        [Obsolete("XAMLIL placeholder", true)]
        public FirstTimeWizard() { }

        public FirstTimeWizard(IConfigurationLoader configuration, IPageNavigationProvider pageNavigator, IFirstTimeWizard wizardService, IFileBrowserFactory fileBrowser)
        {
            _configuration = configuration;
            _pageNavigator = pageNavigator;
            _wizardService = wizardService;
            _fileBrowser = fileBrowser;
            Background = _configuration.UserInterface.TitlebarGradient;

            InitializeComponent();
            RegisterChildren();
        }

        public void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void RegisterChildren()
        {
            BrowseExecutableButton = this.FindControl<Button>(nameof(BrowseExecutableButton));
            BrowseConfigurationButton = this.FindControl<Button>(nameof(BrowseConfigurationButton));
            FinishButton = this.FindControl<Button>(nameof(FinishButton));

            BrowseExecutableButton.Click += BrowseExecutableClick;
            FinishButton.Click += FirstButtonClick;
            BrowseConfigurationButton.Click += BrowseConfigClick;
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
        }

        private void FirstButtonClick(object sender, RoutedEventArgs e)
        {
            _wizardService.WriteInitialConfigs();
            _pageNavigator.Navigate<GameLibrary>();
        }
    }
}