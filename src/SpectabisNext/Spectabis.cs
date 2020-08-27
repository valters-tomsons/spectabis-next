using System.Diagnostics;
using System.Reflection;
using Avalonia.Controls;
using SpectabisLib.Repositories;
using SpectabisUI.Views;
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
            appInstance.Run(_mainWindow);

            // _mainWindow.Show();
            // appInstance.Run(_cancelRepo.GetToken(SpectabisLib.Enums.CancellationTokenKey.SpectabisApp));
        }

        public string GetVersion()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var fileVersion = fileInfo.FileVersion;

            if(fileVersion == "0.0.0.0")
            {
                return "develop";
            }

            return fileInfo.FileVersion;
        }
    }
}