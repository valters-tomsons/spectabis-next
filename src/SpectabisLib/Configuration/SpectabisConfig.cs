using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class SpectabisConfig : IJsonConfig
    {
        public string FileName => nameof(SpectabisConfig).ConfigClassToFileName();

        public SpectabisConfig()
        {
            RunFirstTimeWizard = true;
            EnableDiscordRichPresence = true;
            EnableTelemetry = true;
        }

        public bool RunFirstTimeWizard { get; set; }
        public bool EnableDiscordRichPresence { get; set; }
        public bool EnableTelemetry { get; set; }
    }
}