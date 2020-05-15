using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisService.Services;

namespace SpectabisService
{
    public static class GetGameBoxArt
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly IGameArtClient _artClient;
        private static readonly PCSX2DatabaseProvider _dbProvider = new PCSX2DatabaseProvider(_httpClient);
        private static readonly ContentDownloader _downloader = new ContentDownloader(_httpClient);
        private static readonly StorageProvider _storage = new StorageProvider();

        static GetGameBoxArt()
        {
            _artClient = new GiantBombClient();
        }

        [FunctionName("GetGameBoxArt")]
        public static async Task<IActionResult> Run(
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

            await _storage.InitializeStorage().ConfigureAwait(false);
            var cached = await _storage.GetFromCache(normalizedSerial).ConfigureAwait(false);

            if (cached != null)
            {
                return new FileContentResult(cached, "image/png");
            }

            var artUrl = await _artClient.GetBoxArt(game.Title).ConfigureAwait(false);
            var result = await _downloader.DownloadGameArt(artUrl).ConfigureAwait(false);

            await _storage.WriteToCache(normalizedSerial, result).ConfigureAwait(false);

            return new FileContentResult(result, "image/png");
        }
    }
}