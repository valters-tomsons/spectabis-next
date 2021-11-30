using System;
using SpectabisLib.Enums;

namespace SpectabisLib.Models
{
    public class GameProfile
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string EmulatorPath { get; set; } = string.Empty;
        public string BoxArtPath { get; set; } = string.Empty;
        public EmulatorLaunchOptions LaunchOptions { get; set; }
        public TimeSpan Playtime { get; set; }
        public DateTimeOffset LastPlayed { get; set; }
    }
}