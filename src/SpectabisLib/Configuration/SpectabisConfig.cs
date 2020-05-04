using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class SpectabisConfig : IJsonConfig
    {
        public SpectabisConfig()
        {
            Title = "Spectabis";
            RunFirstTimeWizard = true;
        }

        public string Title { get; }
        public bool RunFirstTimeWizard { get; set; }
    }
}