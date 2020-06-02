using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        GameProcess StartGame(GameProfile game);
        GameProcess StartConfiguration(GameProfile game);
        Task StopGame();
        GameProcess GetRunningGame();
    }
}