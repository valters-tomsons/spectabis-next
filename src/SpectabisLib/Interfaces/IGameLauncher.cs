using System.Diagnostics;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameLauncher
    {
        string EmulatorPath { get; set; }
        Process Launch(GameProfile game);
        void BeginShutdown();

    }
}