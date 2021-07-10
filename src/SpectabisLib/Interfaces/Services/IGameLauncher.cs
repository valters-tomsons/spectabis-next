using System.Threading.Tasks;
using SpectabisLib.Models;
using SpectabisLib.Abstractions;
using SpectabisLib.Interfaces.Abstractions;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        Task<IGameProcess> StartGame(GameProfile game);
        IGameProcess StartConfiguration(GameProfile game);
        Task StopGame();
        IGameProcess GetRunningGame();
    }
}