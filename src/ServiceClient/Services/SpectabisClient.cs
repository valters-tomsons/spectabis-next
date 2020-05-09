using System;
using System.Diagnostics;
using System.Security;
using System.Threading.Tasks;
using ServiceClient.Helpers;
using ServiceClient.Interfaces;

namespace ServiceClient.Services
{
    public class SpectabisClient : ISpectabisClient
    {
        private Uri ServiceBaseUrl { get; } = new Uri("https://spectabis-api-eu.azurewebsites.net/api/", UriKind.Absolute);
        private Uri GetArtEndpoint { get; } = new Uri("GetGameBoxArt", UriKind.Relative);
        private readonly string ServiceApiKey = ServiceCredentialsHelper.ApiKey;

        private readonly IRestClient _restClient;

        public SpectabisClient(IRestClient restClient)
        {
            _restClient = restClient;
            _restClient.SetSession(ServiceApiKey, ServiceBaseUrl);
        }

        public async Task<Uri> GetBoxArtUrl(string serial)
        {
            var response = await _restClient.GetFunctionRequest(GetArtEndpoint, $"serial={serial}").ConfigureAwait(false);
            response.Body = response.Body.Replace("\"", string.Empty);
            var isValidUri = Uri.TryCreate(response.Body, UriKind.Absolute, out var result);

            return isValidUri ? result : null;
        }
    }
}
