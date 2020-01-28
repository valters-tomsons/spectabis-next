using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        private Button FirstButton;

        [Obsolete("XAMLIL placeholder", true)]
        public FirstTimeWizard() { }

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