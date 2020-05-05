using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        private Button FirstButton;

        [Obsolete("XAMLIL placeholder", true)]
        public FirstTimeWizard() { }

        public FirstTimeWizard(IConfigurationLoader configuration, IPageNavigationProvider pageNavigator, IFirstTimeWizard wizardService)
        {
            _configuration = configuration;
            _pageNavigator = pageNavigator;
            _wizardService = wizardService;
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
            _wizardService.WriteInitialConfigs();
            _pageNavigator.Navigate<GameLibrary>();
        }
    }
}