using System;
using System.Diagnostics;
using System.IO;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        private GameProcess _gameProcess;
        
        public GameLauncherPCSX2()
        {

        }

        public GameProcess StartGame(GameProfile game)
        {
            var process = CreateEmulatorProcess(game);
            var gameProcess = new GameProcess(game, process);

            _gameProcess = gameProcess;
            _gameProcess.Start();

            return gameProcess;
        }

        public GameProcess GetRunningGame()
        {
            return _gameProcess;
        }

        public void StopGame()
        {
            _gameProcess = null;
            _gameProcess.Stop();
        }

        private Process CreateEmulatorProcess(GameProfile gameProfile)
        {
            if(_gameProcess != null)
            {
                Console.WriteLine($"[PCSX2-GameLauncher] Losing lease of existing process '{_gameProcess.Game.Title}' : '{_gameProcess.Process.Id}'");
            }

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
            if (string.IsNullOrWhiteSpace(profile.EmulatorPath))
            {
                return SystemDirectories.PCSX2ExecutablePath;
            }

            return profile.EmulatorPath;
        }

        private bool IsGameFileValid(string gamePath)
        {
            if (!String.IsNullOrEmpty(gamePath) && File.Exists(gamePath)) { return true; }
            return false;
        }

    }
}