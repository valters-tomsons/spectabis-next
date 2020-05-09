using System;

namespace ServiceClient.Helpers
{
    public static class ServiceCredentialsHelper
    {
        private const string _apiKeyTokenized = "{{ServiceApiKey}}";

        public static string ApiKey
        {
            get
            {
                return GetApiKey();
            }
        }

        private static string GetApiKey()
        {
            if (_apiKeyTokenized == string.Concat("{{", "ServiceApiKey", "}}"))
            {
                Console.WriteLine("[CredentialsHelper] WARNING! No API key has been inserted into source code.");
                Console.WriteLine("[CredentialsHelper] Trying to use api key from 'SERVICE_API_KEY' variable.");
                return Environment.GetEnvironmentVariable("SERVICE_API_KEY");
            }

            return _apiKeyTokenized;
        }
    }
}