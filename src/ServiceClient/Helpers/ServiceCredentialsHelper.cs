using System;

namespace ServiceClient.Helpers
{
    public static class ServiceCredentialsHelper
    {
        private const string _spectabisApiKey = "{{ServiceApiKey}}";

        public static string ApiKey
        {
            get
            {
                return GetApiKey();
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
    }
}