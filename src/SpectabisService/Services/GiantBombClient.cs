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

        public async Task<Uri> GetBoxArt(string titleQuery)
        {
            var result = await _client.SearchForGamesAsync(titleQuery).ConfigureAwait(false);
            var firstGame = result.First();
            return new Uri(firstGame.Image.SmallUrl, UriKind.Absolute);
        }
    }
}