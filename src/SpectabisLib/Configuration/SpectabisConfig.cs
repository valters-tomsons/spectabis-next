using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class SpectabisConfig : IJsonConfig
    {
        public SpectabisConfig()
        {
            RunFirstTimeWizard = true;
            EnableDiscordRichPresence = true;
            EnableTelemetry = true;
        }

        public string Title { get; } = nameof(SpectabisConfig).ConfigClassToFileName();
        public bool RunFirstTimeWizard { get; set; }
        public bool EnableDiscordRichPresence { get; set; }
        public bool EnableTelemetry { get; set; }
    }
}