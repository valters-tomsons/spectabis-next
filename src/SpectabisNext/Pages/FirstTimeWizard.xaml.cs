using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Pages
{
    public class FirstTimeWizard : UserControl, Page
    {
        private string pageTitle = "First Time Wizard";
        private bool showInTitlebar = false;
        private bool hideTitlebar = true;
        private bool reloadOnNavigation = true;

        public string PageTitle { get { return pageTitle; } }
        public bool ShowInTitlebar { get { return showInTitlebar; } }
        public bool HideTitlebar { get { return hideTitlebar; } }
        public bool ReloadOnNavigation { get { return reloadOnNavigation; } }

        private readonly IConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _pageNavigator;
        private Button FirstButton;

        public FirstTimeWizard()
        {}


        public FirstTimeWizard(IConfigurationLoader configuration, IPageNavigationProvider pageNavigator)
        {
            _configuration = configuration;
            _pageNavigator = pageNavigator;
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
            FirstButton = this.FindControl<Button>("firstButton");
            FirstButton.Click += FirstButtonClick;
        }

        private void FirstButtonClick(object sender, RoutedEventArgs e)
        {
            _pageNavigator.Navigate<GameLibrary>();
        }
    }
}