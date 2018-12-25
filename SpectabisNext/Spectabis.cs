using SpectabisNext.Interfaces;
using SpectabisNext.Views;

namespace SpectabisNext
{
    public class Spectabis : ISpectabis
    {
        private readonly IWindowConfiguration _windowConfiguration;
        private readonly MainWindow _mainWindow;

        public Spectabis(IWindowConfiguration windowConfiguration, MainWindow mainWindow)
        {
            _windowConfiguration = windowConfiguration;
            _mainWindow = mainWindow;
        }

        public void Start()
        {
            var appInstance = _windowConfiguration.GetInstance();
            appInstance.Run(_mainWindow);
        }

    }
}