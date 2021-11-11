using System;
using System.Threading.Tasks;
using Common.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Controllers;
using SpectabisUI.Interfaces;
using SpectabisUI.Pages;

namespace SpectabisUI.Controllers
{
    public class GameSettingsController : IGameSettingsController
    {
        private readonly IGameLauncher _gameLauncher;
        private readonly IProfileRepository _profileRepository;
        private readonly IPageNavigationProvider _navigationProvider;

        public GameSettingsController(IGameLauncher gameLauncher, IProfileRepository profileRepository, IPageNavigationProvider navigationProvider)
        {
            _gameLauncher = gameLauncher;
            _profileRepository = profileRepository;
            _navigationProvider = navigationProvider;
        }

        public async Task LaunchConfiguration(Guid gameId)
        {
            var game = await _profileRepository.Get(gameId).ConfigureAwait(true);
            Logging.WriteLine($"Opening PCSX2 configuration for {gameId} : '{game.Title}'");
            _gameLauncher.StartConfiguration(game);
            _navigationProvider.Navigate<GameRunning>();
        }
    }
}