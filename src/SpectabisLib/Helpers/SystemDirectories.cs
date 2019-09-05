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
        public static readonly string ProfileFolder = $"{HomePath}/.config/spectabis/profiles";
        public static readonly string ConfigFile = $"{ConfigFolder}/spectabis.json";
        public static readonly string ResourcesPath = "Resources";
        //UPDATE ME LATER!
        public static readonly string PCSX2ConfigurationPath = "./PCSX2";
        //UPDATE ME LATER!
        public static readonly string PCSX2ExecutablePath = "PCSX2";
        //UPDATE ME LATER!
        public static readonly string PCSX2GameDatabasePath = "./PCSX2/gameindex.csv";
    }
}