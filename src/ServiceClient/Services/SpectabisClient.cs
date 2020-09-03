using System;
using System.Threading.Tasks;
using ServiceClient.Helpers;
using ServiceClient.Interfaces;

namespace ServiceClient.Services
{
    public class SpectabisClient : ISpectabisClient
    {
        private Uri ServiceBaseUrl { get; } = new Uri(ServiceCredentialsHelper.ServiceBaseUrl, UriKind.Absolute);
        private Uri GetArtEndpoint { get; } = new Uri("GetGameBoxArt", UriKind.Relative);

        private readonly IRestClient _restClient;

        public SpectabisClient(IRestClient restClient)
        {
            Console.WriteLine($"[ServiceClient] Creating client to service at '{ServiceBaseUrl}'");

            _restClient = restClient;
            _restClient.SetSession(ServiceCredentialsHelper.ServiceApiKey, ServiceBaseUrl);
        }

        public async Task<byte[]> DownloadBoxArt(string serial)
        {
            return await _restClient.GetBytesAsync(GetArtEndpoint, $"serial={serial}").ConfigureAwait(false);
        }
    }
}