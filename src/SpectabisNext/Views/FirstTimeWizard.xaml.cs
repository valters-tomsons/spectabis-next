using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SpectabisNext.Services;
using SpectabisUI.Controls;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Views
{
    public class FirstTimeWizard : Page
    {
        private readonly ConfigurationLoader _configuration;
        private readonly IPageNavigationProvider _pageNavigator;
        private Button FirstButton;

        public FirstTimeWizard(ConfigurationLoader configuration, IPageNavigationProvider pageNavigator)
        {
            _configuration = configuration;
            _pageNavigator = pageNavigator;
            Background = _configuration.UserInterface.TitlebarGradient;
            HideTitlebar = true;

            RegisterChildren();
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