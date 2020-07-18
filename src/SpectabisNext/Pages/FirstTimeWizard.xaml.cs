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
        private Button FinishButton;
        private Button BrowseExecutable;

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
            BrowseExecutable = this.FindControl<Button>(nameof(BrowseExecutable));
            FinishButton = this.FindControl<Button>(nameof(FinishButton));

            BrowseExecutable.Click += BrowseExecutableClick;
            FinishButton.Click += FirstButtonClick;
        }

        private void BrowseExecutableClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(Open_BrowseExectuable);
        }

        private async Task Open_BrowseExectuable()
        {
            var oldPath = _configuration.Directories.PCSX2Executable;
            var browserResult = await _fileBrowser.BeginGetSingleFilePath("Select PCSX2 Path", oldPath.LocalPath).ConfigureAwait(false);

            if(string.IsNullOrEmpty(browserResult))
            {
                return;
            }

            _configuration.Directories.PCSX2Executable = new Uri(browserResult, UriKind.Absolute);
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(false);
        }

        private void FirstButtonClick(object sender, RoutedEventArgs e)
        {
            _wizardService.WriteInitialConfigs();
            _pageNavigator.Navigate<GameLibrary>();
        }
    }
}