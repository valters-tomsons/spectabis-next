using System;

namespace SpectabisLib
{
    public static class SystemDirectories
    {
        public static readonly string HomePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX) ?
            Environment.GetEnvironmentVariable("HOME") :
            Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
    }
}