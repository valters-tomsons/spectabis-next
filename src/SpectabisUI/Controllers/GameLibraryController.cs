using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Controllers;
using SpectabisLib.Models;
using SpectabisUI.Interfaces;
using SpectabisUI.Pages;

namespace SpectabisUI.Controllers
{
    public class GameLibraryController : IGameLibraryController
    {
        private readonly IGameLauncher _gameLauncher;
        private readonly IDiscordService _discordService;
        private readonly IProfileRepository _profileRepository;
        private readonly IPageRepository _pageRepository;

        public GameLibraryController(IGameLauncher gameLauncher, IDiscordService discordService, IProfileRepository profileRepository, IPageRepository pageRepository)
        {
            _gameLauncher = gameLauncher;
            _discordService = discordService;
            _profileRepository = profileRepository;
            _pageRepository = pageRepository;
        }

        public void LaunchGame(GameProfile game)
        {
            Logging.WriteLine($"Launching {game.Title}");

            _discordService.SetGamePresence(game);
            _gameLauncher.StartGame(game);
        }

        public void LaunchConfiguration(GameProfile game)
        {
            Logging.WriteLine($"Configuring {game.Title}");
            _gameLauncher.StartConfiguration(game);
        }

        public void DeleteGame(GameProfile game)
        {
            Logging.WriteLine($"Removing {game.Id}");
            _profileRepository.DeleteProfile(game);
        }

        public void OpenWikiPage(GameProfile game)
        {
            var titleQuery = new StringBuilder(game.Title);
            titleQuery.Replace(" - ", ":+");
            titleQuery.Replace(" ", "+");
            titleQuery.Replace("++", ":+");

            var wikiUrl = new Uri($"http://wiki.pcsx2.net/index.php?search={titleQuery}", UriKind.Absolute);
            BrowserProvider.OpenWebBrowser(wikiUrl);
        }

        /// <summary>
        /// Returns a list of new games which are not currently displayed in the library view.
        /// </summary>
        public async Task<IEnumerable<GameProfile>> GetNewGames(IList<GameProfile> currentProfiles)
        {
            var allGames = await _profileRepository.GetAll().ConfigureAwait(true);
            return allGames.Except(currentProfiles);
        }

        public object GetConfigureGamePage(GameProfile profile)
        {
            var configurePage = (GameSettings) _pageRepository.GetPage<GameSettings>();
            configurePage.ConfigureProfile(profile);
            return configurePage;
        }
    }
}