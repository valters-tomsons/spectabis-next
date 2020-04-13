using SpectabisLib.Enums;

namespace SpectabisLib.Models
{
    public class GameMetadata
    {
        public string Serial { get; set; }
        public GameCompatibility Compatibility { get; set; }
        public string Title { get; set; }
        public string Region { get; set; }
    }
}