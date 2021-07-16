using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SpectabisLib.Abstractions;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameLauncherPCSX2 : IGameLauncher
    {
        private IGameProcess _gameProcess;
        private readonly IProfileFileSystem _pfs;
        private readonly IConfigurationManager _configLoader;

        public GameLauncherPCSX2(IProfileFileSystem pfs, IConfigurationManager configLoader)
        {
            _pfs = pfs;
            _configLoader = configLoader;
        }

        public async Task<IGameProcess> StartGame(GameProfile game)
        {
            _pfs.GetProfileContainerUriByType(game, ContainerConfigType.Inis);

            var process = CreateEmulatorProcess(game);
            var gameProcess = new GameProcess(game, process);

            _gameProcess = gameProcess;
            _gameProcess.Start();

            await UpdateLastPlayed(game).ConfigureAwait(false);

            return gameProcess;
        }

        public IGameProcess StartConfiguration(GameProfile game)
        {
            var process = CreateEmulatorProcess(game, false);
            var gameProcess = new GameProcess(game, process);

            _gameProcess = gameProcess;
            _gameProcess.Start();

            return gameProcess;
        }

        public IGameProcess GetRunningGame()
        {
            return _gameProcess;
        }

        public async Task StopGame()
        {
            if (_gameProcess == null)
            {
                return;
            }

            _gameProcess.Stop();
            await UpdatePlaytime(_gameProcess).ConfigureAwait(false);
            _gameProcess = null;
        }

        private async Task UpdatePlaytime(IGameProcess process)
        {
            var sessionTime = process.GetElapsed();
            var profile = process.GetGame();

            profile.Playtime += sessionTime;
            await _pfs.CreateOnFileSystem(profile).ConfigureAwait(false);
        }

        private async Task UpdateLastPlayed(GameProfile profile)
        {
            profile.LastPlayed = DateTimeOffset.Now;
            await _pfs.CreateOnFileSystem(profile).ConfigureAwait(false);
        }

        private Process CreateEmulatorProcess(GameProfile gameProfile, bool launchGame = true)
        {
            if (_gameProcess != null)
            {
                var oldGame = _gameProcess.GetGame();
                var oldProcess = _gameProcess.GetProcess();
                Logging.WriteLine($"Losing lease of existing process '{oldGame.Title}' : '{oldProcess.Id}'");
            }

            var process = new Process();

            var launchArguments = EmulatorOptionsParser.ConvertToLaunchArguments(gameProfile.LaunchOptions);
            var cfgArgument = EmulatorOptionsParser.ConfigurationPathToArgument(_pfs.GetProfileContainerUriByType(gameProfile, ContainerConfigType.Inis));
            var romArgument = launchGame ? EmulatorOptionsParser.RomPathToArgument(gameProfile.FilePath) : string.Empty;

            var fullArguments = $"{launchArguments} {romArgument} {cfgArgument}";

            process.StartInfo.FileName = GetEmulatorPath(gameProfile);
            process.StartInfo.Arguments = fullArguments;
            process.StartInfo.RedirectStandardOutput = true;
            process.EnableRaisingEvents = true;

            return process;
        }

        private string GetEmulatorPath(GameProfile profile)
        {
            if (!string.IsNullOrWhiteSpace(profile.EmulatorPath))
            {
                Logging.WriteLine($"Emulator path for '{profile.Title}' loaded from profile.json");
                return profile.EmulatorPath;
            }

            var configValue = _configLoader.Directories.PCSX2Executable.LocalPath;

            if (!string.IsNullOrWhiteSpace(configValue))
            {
                Logging.WriteLine($"Emulator path for '{profile.Title}' loaded from directory.json");
                return configValue;
            }

            Logging.WriteLine($"Emulator path for '{profile.Title}' loaded from default configuration : '{SystemDirectories.Default_PCSX2ExecutablePath}'");
            return SystemDirectories.Default_PCSX2ExecutablePath;
        }
    }
}