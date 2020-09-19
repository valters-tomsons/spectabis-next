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
            await _configLoader.WriteDefaultsIfNotExist<DirectoryStruct>().ConfigureAwait(false);
            await _configLoader.WriteDefaultsIfNotExist<SpectabisConfig>().ConfigureAwait(false);
        }

        public bool IsRequired()
        {
            return true;
        }
    }
}