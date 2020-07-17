using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ServiceClient.Helpers
{
    public static class ServiceCredentialsHelper
    {
        // Parameterized
        public static readonly string ServiceApiKey = "{{ServiceApiKey}}";
        public static readonly string TelemetryKey = "{{TelemetryKey}}";

        private const string LocalSettings = "local.settings.json";

        static ServiceCredentialsHelper()
        {
            if(File.Exists(LocalSettings))
            {
                Console.WriteLine($"Reading settings from '{LocalSettings}'");

                var settingsData = File.ReadAllText(LocalSettings);
                dynamic settings = JToken.Parse(settingsData);

                ServiceApiKey = settings.ServiceApiKey;
                TelemetryKey = settings.TelemetryKey;
            }
        }
    }
}