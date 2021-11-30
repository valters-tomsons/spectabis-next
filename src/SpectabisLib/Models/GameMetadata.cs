using SpectabisLib.Enums;

namespace SpectabisLib.Models
{
    public class GameMetadata
    {
        public string Serial { get; set; } = string.Empty;
        public GameCompatibility Compatibility { get; set; } = GameCompatibility.Unknown;
        public string Title { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
    }
}