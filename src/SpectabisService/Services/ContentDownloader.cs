using System;
using System.Threading.Tasks;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Services.Interfaces;

namespace SpectabisService.Services
{
    public class ContentDownloader : IContentDownloader
    {
        private readonly IHttpClient _client;

        public ContentDownloader(IHttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]?> DownloadGameArt(Uri imageUri)
        {
            var imageResponse = await _client.GetAsync(imageUri).ConfigureAwait(false);

            if(imageResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            return await imageResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }
    }
}