using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Controllers
{
    public interface IGameLibraryController
    {
        void LaunchGame(GameProfile game);
        void LaunchConfiguration(GameProfile game);
        void DeleteGame(GameProfile game);
        void OpenWikiPage(GameProfile game);
        Task<IEnumerable<GameProfile>> GetNewGames(IList<GameProfile> currentProfiles);
        object GetConfigureGamePage(GameProfile profile);
    }
}