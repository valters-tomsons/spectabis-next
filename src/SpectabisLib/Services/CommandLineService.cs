using System;
using System.Threading;
using System.Threading.Tasks;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Services;

namespace SpectabisLib.Services
{
    public class CommandLineService : ICommandLineService
    {
        private readonly IGameLauncher _gameLauncher;
        private readonly IProfileRepository _gameRepository;

        public CommandLineService(IGameLauncher gameLauncher, IProfileRepository gameRepository)
        {
            _gameLauncher = gameLauncher;
            _gameRepository = gameRepository;
        }

        public async Task<bool> ExecuteArguments(string[] arguments)
        {
            if(arguments is null)
            {
                return false;
            }

            if(arguments[0] == "--run")
            {
                _ = await _gameRepository.GetAll().ConfigureAwait(false);
                var game = _gameRepository.Get(new Guid(arguments[1]));

                await _gameLauncher.StartGame(game).ConfigureAwait(false);
                await WaitForExit().ConfigureAwait(false);
                return true;
            }

            return false;
        }

        private async Task WaitForExit(CancellationToken token = default)
        {
            while(_gameLauncher.GetRunningGame()?.GetProcess()?.HasExited == false)
            {
                await Task.Delay(500, token).ConfigureAwait(false);
            }
        }
    }
}