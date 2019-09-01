using System.Diagnostics;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        private Process EmulatorProcess { get; set; }
        public GameLauncherPCSX2()
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

            process.StartInfo.FileName = GetEmulatorPath(gameProfile);
            process.StartInfo.Arguments = fullArguments;
            process.EnableRaisingEvents = true;

            return process;
        }

        private string GetEmulatorPath(GameProfile profile)
        {
            if(string.IsNullOrWhiteSpace(profile.EmulatorPath))
            {
                return SystemDirectories.PCSX2ExecutablePath;
            }

            return profile.EmulatorPath;
        }

    }
}