using SpectabisNext.Controls;
using SpectabisNext.Services;
using SpectabisUI.Controls;

namespace SpectabisNext.Views
{
    public class FirstTimeWizard : Page
    {
        private readonly ConfigurationLoader _configuration;

        public FirstTimeWizard(ConfigurationLoader configuration)
        {
            _configuration = configuration;
            Background = _configuration.UserInterface.TitlebarGradient;
            HideTitlebar = true;
        }

    }
}