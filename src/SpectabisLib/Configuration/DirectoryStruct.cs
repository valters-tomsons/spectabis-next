using System;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class DirectoryStruct : IJsonConfig
    {
        public DirectoryStruct()
        {
            PCSX2ConfigurationPath = new Uri(SystemDirectories.Default_PCSX2ConfigurationPath, UriKind.Absolute);
            PCSX2Executable = new Uri(SystemDirectories.Default_PCSX2ExecutablePath, UriKind.Absolute);
        }

        public string Title { get; } = "Directory";

        public Uri PCSX2Executable { get; set; }
        public Uri PCSX2ConfigurationPath { get; set; }
    }
}