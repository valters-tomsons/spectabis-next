using System;
using System.Linq;
using System.Threading.Tasks;
using GiantBomb.Api;
using SpectabisLib.Interfaces;

namespace SpectabisService.Services
{
    public class GiantBombClient : IGameArtClient
    {
        private readonly IGiantBombRestClient _client;

        public GiantBombClient(IGiantBombRestClient gbClient)
        {
            _client = gbClient;
        }

        public async Task<Uri> GetBoxArtPS2(string titleQuery)
        {
            var result = await _client.SearchForGamesAsync(titleQuery).ConfigureAwait(false);
            result = result.Where(x => x.Platforms.Any(y => y.Abbreviation == "PS2"));

            var firstGame = result.First();

            if(firstGame == null)
            {
                return null;
            }

            return new Uri(firstGame.Image.SmallUrl, UriKind.Absolute);
        }
    }
}