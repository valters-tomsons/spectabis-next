using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SpectabisLib.Helpers;
using SpectabisService.Services;

namespace SpectabisService
{
    public static class GameArt
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly PCSX2DatabaseProvider _dbProvider = new PCSX2DatabaseProvider(_httpClient);

        [FunctionName("GetGameMetadataBySerial")]
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

            var normalizedSerial = ((string)reqSerial).NormalizeSerial();
            var result = gameDb.FirstOrDefault(x => x.Serial.Equals(normalizedSerial));

            if (result == null)
            {
                return new BadRequestObjectResult("No results");
            }

            return new OkObjectResult(result);
        }
    }
}