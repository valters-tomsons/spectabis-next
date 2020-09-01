using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using SpectabisService.Providers.Interfaces;

namespace SpectabisService.Endpoints
{
    public class GetGameBoxArt
    {
        private readonly IGameArtProvider _artProvider;

        public GetGameBoxArt(IGameArtProvider artProvider)
        {
            _artProvider = artProvider;
        }

        [FunctionName("GetGameBoxArt")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var reqSerial = req.Query["serial"];
            return await _artProvider.GetArtWithSerial(reqSerial).ConfigureAwait(false);
        }
    }
}