using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpectabisLib.Interfaces;
using SpectabisLib.Helpers;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Providers.Interfaces;
using SpectabisService.Services;
using System.Linq;

namespace SpectabisService.Providers
{
    public class GameArtProvider : IGameArtProvider
    {
        private readonly IGameArtClient _artClient;
        private readonly PCSX2DatabaseProvider _dbProvider;
        private readonly ContentDownloader _downloader;
        private readonly IStorageProvider _storage;

        public GameArtProvider(IGameArtClient artClient, PCSX2DatabaseProvider dbProvider, ContentDownloader downloader, IStorageProvider storage)
        {
            _artClient = artClient;
            _dbProvider = dbProvider;
            _downloader = downloader;
            _storage = storage;
        }

        public async Task<IActionResult> GetArtWithSerial(string serial)
        {
            if (serial.Length < 1)
            {
                return new BadRequestObjectResult("Missing serial in query");
            }

            var gameDb = await _dbProvider.GetDatabase().ConfigureAwait(false);

            if (gameDb == null)
            {
                return new BadRequestObjectResult("Failed retrieving database");
            }

            var normalizedSerial = serial.NormalizeSerial();
            var game = gameDb.FirstOrDefault(x => x.Serial.Equals(normalizedSerial));

            if (game == null)
            {
                return new BadRequestObjectResult("Unknown game");
            }

            var cached = await _storage.ReadBytesFromStorage(normalizedSerial).ConfigureAwait(false);

            if (cached != null)
            {
                return new FileContentResult(cached, "image/png");
            }

            var artUrl = await _artClient.GetBoxArtPS2(game.Title).ConfigureAwait(false);
            var result = await _downloader.DownloadGameArt(artUrl).ConfigureAwait(false);

            await _storage.WriteImageToStorage(normalizedSerial, result).ConfigureAwait(false);

            return new FileContentResult(result, "image/png");
        }
    }
}