using System.Threading;
using Avalonia.Controls;
using SpectabisNext.Views;
using SpectabisUI.Interfaces;

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
            var cts = new CancellationTokenSource();
            var appInstance = _windowConfiguration.GetInstance();

            _mainWindow.Show();
            appInstance.Run(cts.Token);
        }

    }
}