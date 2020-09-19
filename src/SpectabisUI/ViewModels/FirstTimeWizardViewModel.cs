using ReactiveUI;
using SpectabisLib.Interfaces;

namespace SpectabisUI.ViewModels
{
    public class FirstTimeWizardViewModel : ReactiveObject
    {
        private readonly IConfigurationLoader _configuration;

        public FirstTimeWizardViewModel(IConfigurationLoader configuration)
        {
            _configuration = configuration;

            executablePath = _configuration.Directories.PCSX2Executable.LocalPath;
            configurationPath = _configuration.Directories.PCSX2ConfigurationPath.LocalPath;
        }

        private string executablePath;
        private string configurationPath;
        private string telemetryMessage;

        public string ExecutablePath { get => executablePath; set => this.RaiseAndSetIfChanged(ref executablePath, value); }
        public string ConfigurationPath { get => configurationPath; set => this.RaiseAndSetIfChanged(ref configurationPath, value); }
        public string TelemetryMessage { get => telemetryMessage; set => this.RaiseAndSetIfChanged(ref telemetryMessage, value); }
    }
}