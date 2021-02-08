using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ITelemetry _telemetry;

        public SpectabisClient(IRestClient restClient, ITelemetry telemetry)
        {
            Console.WriteLine($"[ServiceClient] Creating client to service at '{ServiceBaseUrl}'");

            _restClient = restClient;
            _restClient.SetSession(ServiceCredentialsHelper.ServiceApiKey, ServiceBaseUrl);

            _telemetry = telemetry;
        }

        public async Task<byte[]> DownloadBoxArt(string serial)
        {
            var result = await _restClient.GetBytesAsync(GetArtEndpoint, $"serial={serial}").ConfigureAwait(false);

            if(result == null)
            {
                _telemetry.TrackFailedClientOperation(Enums.ClientOperation.BoxArtDownload, new Dictionary<string, string> { { nameof(serial), serial } });
                return null;
            }

            if(!ValidateImage(result))
            {
                _telemetry.TrackFailedClientOperation(Enums.ClientOperation.BoxArtValidation, new Dictionary<string, string> { { nameof(serial), serial } });
                return null;
            }

            return result;
        }

        private bool ValidateImage(byte[] imageBuffer)
        {
            var headerBytes = imageBuffer.Take(16).ToArray();
            var header = System.Text.Encoding.ASCII.GetString(headerBytes);
            return header.Contains("PNG") || header.Contains("JFIF");
        }
    }
}