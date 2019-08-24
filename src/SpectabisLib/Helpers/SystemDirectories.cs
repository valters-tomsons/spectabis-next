using System;

namespace SpectabisLib.Helpers
{
    public static class SystemDirectories
    {
        private static readonly string HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX) ?
            Environment.GetEnvironmentVariable("HOME") :
            Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        
        public static readonly string ConfigFolder = $"{HomePath}/.config/spectabis";
        public static readonly string ConfigFile = $"{ConfigFolder}/spectabis.json";
        public static readonly string ResourcesPath = "Resources";
    }
}