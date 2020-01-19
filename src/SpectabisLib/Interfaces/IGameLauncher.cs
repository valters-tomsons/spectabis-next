using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        GameProcess StartGame(GameProfile game);
        // void PauseGame();
        void StopGame();
        GameProcess GetRunningGame();
    }
}