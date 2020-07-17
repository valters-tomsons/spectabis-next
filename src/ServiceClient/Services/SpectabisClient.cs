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

        private readonly IRestClient _restClient;

        public SpectabisClient(IRestClient restClient)
        {
            _restClient = restClient;
            _restClient.SetSession(ServiceCredentialsHelper.ServiceApiKey, ServiceBaseUrl);
        }

        public async Task<byte[]> DownloadBoxArt(string serial)
        {
            return await _restClient.GetBytesAsync(GetArtEndpoint, $"serial={serial}").ConfigureAwait(false);
        }
    }
}