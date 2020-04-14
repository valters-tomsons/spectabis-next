using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        GameProcess StartGame(GameProfile game);
        GameProcess StartConfiguration(GameProfile game);
        void StopGame();
        GameProcess GetRunningGame();
    }
}