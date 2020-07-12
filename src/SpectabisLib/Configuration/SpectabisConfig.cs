using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class SpectabisConfig : IJsonConfig
    {
        public SpectabisConfig()
        {
            Title = "Spectabis";
            RunFirstTimeWizard = true;
            EnableDiscordRichPresence = true;
            EnableTelemetry = true;
        }

        public string Title { get; }
        public bool RunFirstTimeWizard { get; set; }
        public bool EnableDiscordRichPresence { get; set; }
        public bool EnableTelemetry { get; set; }
    }
}