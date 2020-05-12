using System;
using System.Net.Http;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisService.Services
{
    public class ContentDownloader
    {
        private readonly HttpClient _client;

        public ContentDownloader(HttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]> DownloadGameArt(Uri imageUri)
        {
            var imageResponse = await _client.GetAsync(imageUri).ConfigureAwait(false);
            return await imageResponse.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
        }
    }
}