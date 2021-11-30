namespace ServiceLib.Models
{
    public class LocalCredentials
    {
        public string ServiceApiKey { get; set; } = string.Empty;
        public string TelemetryKey { get; set; } = string.Empty;
        public string ServiceBaseUrl { get; set; } = string.Empty;
    }
}