using System;

namespace ServiceClient.Helpers
{
    public static class ServiceCredentialsHelper
    {
        // Parameterized
        private const string _spectabisApiKey = "{{ServiceApiKey}}";
        private const string _telemetryKey = "{{TelemetryKey}}";

        public static string ApiKey
        {
            get
            {
                return GetApiKey();
            }
        }

        public static string TelemetryInstrumentationKey
        {
            get
            {
                return GetTelemetryKey();
            }
        }

        private static string GetApiKey()
        {
            if (_spectabisApiKey == string.Concat("{{", "ServiceApiKey", "}}"))
            {
                Console.WriteLine("[CredentialsHelper] Trying to use api key from 'SERVICE_API_KEY' variable.");
                var key = Environment.GetEnvironmentVariable("SERVICE_API_KEY");
                return key;
            }

            return _spectabisApiKey;
        }

        private static string GetTelemetryKey()
        {
            return _telemetryKey;
        }
    }
}