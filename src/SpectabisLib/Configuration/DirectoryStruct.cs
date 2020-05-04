using System;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class DirectoryStruct : IJsonConfig
    {
        public string Title { get; } = "Directory";

        public Uri PCSX2Executable { get; set; }
        public Uri PCSX2ConfigurationPath { get; set; }
    }
}