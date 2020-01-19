using System.Threading;
using Avalonia.Controls;
using SpectabisLib.Repositories;
using SpectabisNext.Views;
using SpectabisUI.Interfaces;

namespace SpectabisNext
{
    public class Spectabis : ISpectabis
    {
        private readonly IWindowConfiguration _windowConfiguration;
        private readonly MainWindow _mainWindow;
        private readonly CancellationTokenRepository _cancelRepo;

        public Spectabis(IWindowConfiguration windowConfiguration, MainWindow mainWindow, CancellationTokenRepository cancelRepo)
        {
            _windowConfiguration = windowConfiguration;
            _mainWindow = mainWindow;
            _cancelRepo = cancelRepo;
        }

        public void Start()
        {
            var appInstance = _windowConfiguration.GetInstance();

            _mainWindow.Show();
            appInstance.Run(_cancelRepo.GetToken(SpectabisLib.Enums.CancellationTokenKey.SpectabisApp));
        }

    }
}