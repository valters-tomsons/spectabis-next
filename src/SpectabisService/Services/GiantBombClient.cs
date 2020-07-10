using System;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api;
using Microsoft.Extensions.Configuration;
using SpectabisLib.Interfaces;

namespace SpectabisService.Services
{
    public class GiantBombClient : IGameArtClient
    {
        private readonly IGiantBombRestClient _client;

        public GiantBombClient(IConfigurationRoot config)
        {
            var apiKey = config.GetValue<string>("ApiKey_GiantBomb");
            _client = new GiantBombRestClient(apiKey);
        }

        public async Task<Uri> GetBoxArtPS2(string titleQuery)
        {
            var queryResult = await _client.SearchForGamesAsync(titleQuery).ConfigureAwait(false);
            var result = queryResult.FirstOrDefault( x => x.Platforms.Any(y => y.Abbreviation == "PS2"));

            if(result == null)
            {
                return null;
            }

            return new Uri(result.Image.SmallUrl, UriKind.Absolute);
        }
    }
}