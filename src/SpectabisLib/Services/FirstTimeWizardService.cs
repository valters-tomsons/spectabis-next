using System.Threading.Tasks;
using SpectabisLib.Configuration;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Services
{
    public class FirstTimeWizardService : IFirstTimeWizardService
    {
        private readonly IConfigurationLoader _configLoader;

        public FirstTimeWizardService(IConfigurationLoader configLoader)
        {
            _configLoader = configLoader;
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
    }
}