using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using SpectabisLib.Configuration;
using SpectabisLib.Interfaces;

namespace SpectabisUI.ViewModels
{
    public class SettingsViewModel : ReactiveObject
    {
        private readonly IConfigurationManager _configuration;
        private IList<string> _directories;

        public SpectabisConfig Config => _configuration.Spectabis;

        public SettingsViewModel(IConfigurationManager configuration)
        {
            _configuration = configuration;
            _directories = _configuration.Directories.GameScanDirectories.ToList();

            telemetry = _configuration.Spectabis.EnableTelemetry;
            discord = _configuration.Spectabis.EnableDiscordRichPresence;
        }

        private bool telemetry;
        private bool discord;

        public IList<string> ScanDirectories { get => _directories; set => this.RaiseAndSetIfChanged(ref _directories, value); }
        public bool Telemetry { get => telemetry; set => this.RaiseAndSetIfChanged(ref telemetry, value); }
        public bool Discord { get => discord; set => this.RaiseAndSetIfChanged(ref discord, value); }
    }
}