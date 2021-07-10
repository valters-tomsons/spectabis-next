using System;
using System.Threading;
using System.Threading.Tasks;
using SpectabisLib.Helpers;
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

        /// <summary>
        /// Executes commandline arguments, then returns `true` if graphical interface should not be started
        /// </summary>
        public async Task<bool> ExecuteArguments(string[] arguments)
        {
            if(arguments is null)
            {
                return false;
            }

            if(arguments[0] == "--run" || arguments[0] == "-r" || arguments[0] == "-profile")
            {
                var game = await _gameRepository.Get(new Guid(arguments[1])).ConfigureAwait(false);

                await _gameLauncher.StartGame(game).ConfigureAwait(false);
                await WaitForExit().ConfigureAwait(false);
                return true;
            }
            else
            {
                Logging.WriteLine("Unknown command line argument!");
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