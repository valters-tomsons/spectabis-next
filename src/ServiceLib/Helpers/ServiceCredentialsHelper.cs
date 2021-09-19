using System.Diagnostics;
using System;
using System.IO;
using Newtonsoft.Json;
using ServiceLib.Models;

namespace ServiceLib.Helpers
{
    public static class ServiceCredentialsHelper
    {
        // Parameterized
        public static readonly string ServiceApiKey = "{{SERVICE_API_KEY}}";
        public static readonly string TelemetryKey = "{{TELEMETRY_KEY}}";
        public static string ServiceBaseUrl { get; private set; } = "{{SERVICE_BASE_URL}}";

        private const string LocalSettings = "local.settings.json";

        static ServiceCredentialsHelper()
        {
            if(File.Exists(LocalSettings))
            {
                Console.WriteLine($"Reading settings from '{LocalSettings}'");

                var settingsData = File.ReadAllText(LocalSettings);
                var settings = JsonConvert.DeserializeObject<LocalCredentials>(settingsData);

                ServiceApiKey = settings.ServiceApiKey;
                TelemetryKey = settings.TelemetryKey;
                ServiceBaseUrl = settings.ServiceBaseUrl;
            }
            else
            {
                DebugDefaults();
            }
        }

        [Conditional("DEBUG")]
        private static void DebugDefaults()
        {
            ServiceBaseUrl = "http://localhost:7071/api/";
        }
    }
}