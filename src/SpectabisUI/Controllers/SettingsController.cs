using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SpectabisLib.Interfaces;

namespace SpectabisUI.Controllers
{
    public class SettingsController : ISettingsController
    {
        private readonly IConfigurationManager _configuration;

        public SettingsController(IConfigurationManager configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<string>> AppendScanDirectory(string updated)
        {
            if(string.IsNullOrEmpty(updated))
            {
                return null;
            }

            if(!Directory.Exists(updated))
            {
                return null;
            }

            _configuration.Directories.GameScanDirectories = _configuration.Directories.GameScanDirectories.Append(updated);
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);
            return _configuration.Directories.GameScanDirectories;
        }

        public async Task<IEnumerable<string>> RemoveScanDirectory(string target)
        {
            _configuration.Directories.GameScanDirectories = _configuration.Directories.GameScanDirectories.Where(x => x != target);
            await _configuration.WriteConfiguration(_configuration.Directories).ConfigureAwait(true);
            return _configuration.Directories.GameScanDirectories;
        }

        public async Task UpdateOptions(bool telemetry, bool discord)
        {
            _configuration.Spectabis.EnableTelemetry = telemetry;
            _configuration.Spectabis.EnableDiscordRichPresence = discord;

            await _configuration.WriteConfiguration(_configuration.Spectabis).ConfigureAwait(false);
        }
    }
}