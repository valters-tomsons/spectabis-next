using System;
using System.Runtime.InteropServices;

namespace SpectabisLib.Helpers
{
    public static class SystemDirectories
    {
        public static string ConfigFolder { get; private set; }
        public static string ProfileFolder { get; private set; }
        public static string Default_PCSX2ConfigurationPath { get; private set; }
        public static string Default_PCSX2ExecutablePath { get; private set; }
        public static string ResourcesPath { get; } = "Resources";
        public static string HomeFolder { get; private set; }
        public static string GlobalConfigsFolder { get; private set; }

        static SystemDirectories()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                InitializeForWindows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                InitializeForUnix();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                InitializeForUnix();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private static void InitializeForUnix()
        {
            var homePath = Environment.GetEnvironmentVariable("HOME");
            HomeFolder = homePath;
            ConfigFolder = $"{homePath}/.config/spectabis";
            ProfileFolder = $"{ConfigFolder}/profiles";
            Default_PCSX2ConfigurationPath = $"{homePath}/.config/PCSX2";
            Default_PCSX2ExecutablePath = "/usr/bin/PCSX2";
            GlobalConfigsFolder = $"{ConfigFolder}/global";
        }

        private static void InitializeForWindows()
        {
            var homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            HomeFolder = homePath;
            ConfigFolder = $"{homePath}/.spectabis";
            ProfileFolder = $"{ConfigFolder}/profiles";
            Default_PCSX2ConfigurationPath = $"{homePath}/PCSX2";
            Default_PCSX2ExecutablePath = "C:/Program Files (x86)/PCSX2/pcsx2.exe";
            GlobalConfigsFolder = $"{ConfigFolder}/global";
        }
    }
}