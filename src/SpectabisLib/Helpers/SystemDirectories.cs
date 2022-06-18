using System;
using System.Runtime.InteropServices;

namespace SpectabisLib.Helpers
{
    public static class SystemDirectories
    {
        public static string Default_PCSX2ConfigurationPath { get; private set; } = string.Empty;
        public static string Default_PCSX2ExecutablePath { get; private set; } = string.Empty;

        public static string ConfigFolder { get; private set; } = string.Empty;
        public static string ProfileFolder { get; private set; } = string.Empty;
        public static string HomeFolder { get; private set; } = string.Empty;
        public static string GlobalConfigsFolder { get; private set; } = string.Empty;
        public static string LocalArtCacheFolder { get; private set; } = string.Empty;

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

            ConfigFolder = $"{homePath}/.config/spectabis/";
            ProfileFolder = $"{ConfigFolder}/profiles/";
            Default_PCSX2ConfigurationPath = $"{homePath}/.config/PCSX2/";
            Default_PCSX2ExecutablePath = "/usr/bin/pcsx2";
            GlobalConfigsFolder = $"{ConfigFolder}/global/";

            LocalArtCacheFolder = $"{homePath}/.cache/spectabis/boxart/";
        }

        private static void InitializeForWindows()
        {
            var homePath = Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
            HomeFolder = homePath;

            ConfigFolder = $"{homePath}/.spectabis/";
            ProfileFolder = $"{ConfigFolder}/profiles/";
            Default_PCSX2ConfigurationPath = $"{homePath}/PCSX2/";
            Default_PCSX2ExecutablePath = "C:/Program Files (x86)/PCSX2/pcsx2.exe";
            GlobalConfigsFolder = $"{ConfigFolder}/global/";

            var tempPath = Environment.ExpandEnvironmentVariables("%TEMP%");
            LocalArtCacheFolder = $"{tempPath}/spectabis/";
        }
    }
}