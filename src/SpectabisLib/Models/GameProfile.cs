using System;
using SpectabisLib.Enums;

namespace SpectabisLib.Models
{
    public class GameProfile
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string SerialNumber { get; set; }
        public string FilePath { get; set; }
        public string EmulatorPath { get; set; }
        public string BoxArtPath { get; set; }
        public EmulatorLaunchOptions LaunchOptions { get; set; }
        public TimeSpan Playtime { get; set; }
        public DateTimeOffset LastPlayed { get; set; }
    }
}