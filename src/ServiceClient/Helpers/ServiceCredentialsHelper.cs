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
            return _apiKeyTokenized;
        }
    }
}