namespace SpectabisLib.Models
{
    public struct SystemPathsStruct
    {
        public SystemPathsStruct(SystemPathsStruct defaultPathsStruct)
        {
            this = defaultPathsStruct;
        }

        public string SpectabisConfigFile { get; set; }
        public string SpectabisConfigFolder { get; set; }
        public string ProfilesFolder { get; set; }
        public string ResourcesFolder { get; set; }

        public string PCSX2ConfigurationFolder { get; set; }
        public string PCSX2ExecutablePath { get; set; }
        public string PCSX2GameDatabaseFilePath { get; set; }
    }
}