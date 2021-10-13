using System;
using System.IO;
using System.Threading.Tasks;
using Common.Helpers;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces.Services;

namespace SpectabisLib.Services
{
    public class LocalCachingService : ILocalCachingService
    {
        private readonly Uri _artCachePath;

        public LocalCachingService()
        {
            _artCachePath = new Uri(SystemDirectories.LocalArtCacheFolder, UriKind.Absolute);
            EnsureCachingFolderExists();
        }

        public async Task<byte[]> GetCachedArt(string gameSerial)
        {
            var artCacheFilePath = new Uri(_artCachePath, $"{gameSerial}.png");

            if(!File.Exists(artCacheFilePath.LocalPath))
            {
                Logging.WriteLine($"No local art cache for {gameSerial}");
                return null;
            }

            return await File.ReadAllBytesAsync(artCacheFilePath.LocalPath).ConfigureAwait(false);
        }

        public async Task WriteArtToCache(string gameSerial, byte[] buffer)
        {
            if(string.IsNullOrWhiteSpace(gameSerial) || buffer == null)
            {
                Logging.WriteLine("Failed to write art cache");
                return;
            }

            var artCacheFilePath = new Uri(_artCachePath, $"{gameSerial}.png");

            Logging.WriteLine($"Writing {gameSerial} art cache");
            await File.WriteAllBytesAsync(artCacheFilePath.LocalPath, buffer).ConfigureAwait(false);
        }

        private void EnsureCachingFolderExists()
        {
            if (!Directory.Exists(_artCachePath.LocalPath))
            {
                Logging.WriteLine($"Creating {nameof(SystemDirectories.LocalArtCacheFolder)}");
                Directory.CreateDirectory(_artCachePath.LocalPath);
            }
        }
    }
}