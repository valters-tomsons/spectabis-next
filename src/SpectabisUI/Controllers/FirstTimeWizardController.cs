using System;
using System.Threading.Tasks;
using SpectabisLib.Abstractions;
using SpectabisLib.Configuration;
using SpectabisLib.Enums;
using SpectabisLib.Interfaces;

namespace SpectabisUI.Controllers
{
    public class FirstTimeWizardController : IFirstTimeWizardService
    {
        private readonly IConfigurationManager _configLoader;
        private readonly ProfileFileSystem _fileSystem;

        public FirstTimeWizardController(IConfigurationManager configLoader, ProfileFileSystem fileSystem)
        {
            _configLoader = configLoader;
            _fileSystem = fileSystem;
        }

        public async Task WriteInitialConfigs()
        {
            await _configLoader.WriteDefaultsIfNotExist<DirectoryConfig>().ConfigureAwait(false);
            await _configLoader.WriteDefaultsIfNotExist<SpectabisConfig>().ConfigureAwait(false);
        }

        public async Task DisableFirstTimeWizard()
        {
            _configLoader.Spectabis.RunFirstTimeWizard = false;
            await _configLoader.WriteConfiguration(_configLoader.Spectabis).ConfigureAwait(false);
        }

        public bool IsRequired()
        {
            return _configLoader.Spectabis.RunFirstTimeWizard;
        }

        public string GetTelemetryStatusMessage(bool toggle = false)
        {
            if(toggle)
            {
                _configLoader.Spectabis.EnableTelemetry = !_configLoader.Spectabis.EnableTelemetry;
                _configLoader.WriteConfiguration(_configLoader.Spectabis);
            }

            var isEnabled = _configLoader.Spectabis.EnableTelemetry;

            if(isEnabled)
            {
                return _configLoader.TextConfig.TelemetryEnabled;
            }
            else
            {
                return _configLoader.TextConfig.TelemetryOptedOut;
            }
        }

        public async Task SaveToGlobalConfiguration()
        {
            var pcsx2InisFolder = new Uri(_configLoader.Directories.PCSX2ConfigurationPath, "inis/");
            await _fileSystem.CopyToGlobalContainer(pcsx2InisFolder, ContainerConfigType.Inis).ConfigureAwait(false);
        }
    }
}