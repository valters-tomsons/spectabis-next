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
            var image = await _client.GetStreamAsync(imageUri).ConfigureAwait(false);
            byte[] imageBuffer = new byte[image.Length];
            await image.ReadAsync(imageBuffer).ConfigureAwait(false);
            return imageBuffer;
        }
    }
}