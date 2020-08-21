using System;
using System.Collections.Generic;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Configuration
{
    public class DirectoryStruct : IJsonConfig
    {
        private string _pcsx2Executable = SystemDirectories.Default_PCSX2ExecutablePath;
        private string _pcsx2ConfigurationPath = SystemDirectories.Default_PCSX2ConfigurationPath;
        private string _lastFileBrowserDirectory = SystemDirectories.HomeFolder;
        private IEnumerable<string> _gameScanDirectories = new List<string>();

        public string Title { get; } = "Directory";

        public Uri PCSX2Executable
        {
            get { return new Uri(_pcsx2Executable, UriKind.Absolute); }
            set { _pcsx2Executable = value.OriginalString; }
        }

        public Uri PCSX2ConfigurationPath
        {
            get { return new Uri(_pcsx2ConfigurationPath, UriKind.Absolute); }
            set { _pcsx2ConfigurationPath = value.OriginalString; }
        }

        public Uri LastFileBrowserDirectory
        {
            get { return new Uri(_lastFileBrowserDirectory, UriKind.Absolute); }
            set { _lastFileBrowserDirectory = value.OriginalString; }
        }

        public IEnumerable<string> GameScanDirectories
        {
            get { return _gameScanDirectories; }
            set { _gameScanDirectories = value;}
        }
    }
}