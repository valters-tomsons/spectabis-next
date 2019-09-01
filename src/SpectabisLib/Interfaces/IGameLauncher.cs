using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        Process Launch(GameProfile game);
        void BeginShutdown();

    }
}