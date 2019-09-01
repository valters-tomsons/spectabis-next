using System.Diagnostics;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        public string EmulatorPath { get; set; }
        public GameLauncherPCSX2()
        {

        }

        public Process Launch(GameProfile game)
        {
            throw new System.NotImplementedException();
        }

        public void BeginShutdown()
        {
            throw new System.NotImplementedException();
        }
    }
}