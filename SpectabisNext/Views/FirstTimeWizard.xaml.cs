using SpectabisNext.Controls;
using SpectabisNext.Configuration;

namespace SpectabisNext.Views
{
    public class FirstTimeWizard : Page
    {
        private readonly UIConfiguration _uIConfiguration;

        public FirstTimeWizard(UIConfiguration uIConfiguration)
        {
            _uIConfiguration = uIConfiguration;
            Background = uIConfiguration.TitlebarGradient;
            HideTitlebar = true;
        }
    }
}