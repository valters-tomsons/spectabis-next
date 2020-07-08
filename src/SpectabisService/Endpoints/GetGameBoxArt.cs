using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Services;

namespace SpectabisService.Endpoints
{
    public class GetGameBoxArt
    {
        private readonly IGameArtClient _artClient;
        private readonly PCSX2DatabaseProvider _dbProvider;
        private readonly ContentDownloader _downloader;
        private readonly IStorageProvider _storage;

        public GetGameBoxArt(IGameArtClient artClient, PCSX2DatabaseProvider dbProvider, ContentDownloader downloader, IStorageProvider storage)
        {
            _artClient = artClient;
            _dbProvider = dbProvider;
            _downloader = downloader;
            _storage = storage;
        }

        [FunctionName("GetGameBoxArt")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var reqSerial = req.Query["serial"];

            if (reqSerial.Count < 1)
            {
                return new BadRequestObjectResult("Missing serial in query");
            }

            var gameDb = await _dbProvider.GetDatabase().ConfigureAwait(false);

            if (gameDb == null)
            {
                return new BadRequestObjectResult("Failed retrieving database");
            }

            var normalizedSerial = ((string) reqSerial).NormalizeSerial();
            var game = gameDb.FirstOrDefault(x => x.Serial.Equals(normalizedSerial));

            if (game == null)
            {
                return new BadRequestObjectResult("Unknown game");
            }

            var cached = await _storage.GetFromCache(normalizedSerial).ConfigureAwait(false);

            if (cached != null)
            {
                return new FileContentResult(cached, "image/png");
            }

            var artUrl = await _artClient.GetBoxArtPS2(game.Title).ConfigureAwait(false);
            var result = await _downloader.DownloadGameArt(artUrl).ConfigureAwait(false);

            await _storage.WriteToCache(normalizedSerial, result).ConfigureAwait(false);

            return new FileContentResult(result, "image/png");
        }
    }
}