using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpectabisLib.Interfaces;
using SpectabisLib.Helpers;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Providers.Interfaces;
using SpectabisService.Services.Interfaces;

namespace SpectabisService.Providers
{
    public class GameArtProvider : IGameArtProvider
    {
        private readonly IGameArtClient _artClient;
        private readonly IGameDatabaseProvider _dbProvider;
        private readonly IContentDownloader _downloader;
        private readonly IStorageProvider _storage;

        public GameArtProvider(IGameArtClient artClient, IGameDatabaseProvider dbProvider, IContentDownloader downloader, IStorageProvider storage)
        {
            _artClient = artClient;
            _dbProvider = dbProvider;
            _downloader = downloader;
            _storage = storage;
        }

        public async Task<IActionResult> GetArtWithSerial(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial) || serial.Length < 1)
            {
                return new BadRequestObjectResult("Missing serial in query");
            }

            await _storage.InitializeStorage().ConfigureAwait(false);

            var normalizedSerial = serial.NormalizeSerial();
            var game = await _dbProvider.GetBySerial(normalizedSerial).ConfigureAwait(false);

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

            if(artUrl == null)
            {
                return new UnprocessableEntityObjectResult("Failed to triangulate game from external providers");
            }

            var result = await _downloader.DownloadGameArt(artUrl).ConfigureAwait(false);

            if(result == null)
            {
                return new NotFoundObjectResult("No boxart is available");
            }

            await _storage.WriteImageToStorage(normalizedSerial, result).ConfigureAwait(false);
            return new FileContentResult(result, "image/png");
        }
    }
}