using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
            var reqTitle = req.Query["title"];

            var gameDb = await _dbProvider.GetDatabase().ConfigureAwait(false);

            var result = gameDb.FirstOrDefault(x => x.Serial.Equals(reqSerial));
            return new OkObjectResult(result);
        }
    }
}