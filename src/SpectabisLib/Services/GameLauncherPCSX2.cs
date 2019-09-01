using System.Diagnostics;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        public string EmulatorPath { get; set; }
        private Process EmulatorProcess { get; set; }
        public GameLauncherPCSX2(string EmulatorPath)
        {

        }

        public Process Launch(GameProfile game)
        {
            EmulatorProcess = CreateEmulatorProcess(game);
            EmulatorProcess.Start();

            return EmulatorProcess;
        }

        public void BeginShutdown()
        {
            KillEmulatorProcess();
        }

        private void KillEmulatorProcess()
        {
            EmulatorProcess.Kill();
        }

        private Process CreateEmulatorProcess(GameProfile gameProfile)
        {
            var process = new Process();

            var launchArguments = EmulatorOptionsParser.ConvertToLaunchArguments(gameProfile.LaunchOptions);
            var romArgument = EmulatorOptionsParser.RomPathToArgument(gameProfile.FilePath);
            var fullArguments = $"{launchArguments} {romArgument}";

            process.StartInfo.FileName = EmulatorPath;
            process.StartInfo.Arguments = fullArguments;
            process.EnableRaisingEvents = true;

            return process;
        }

    }
}