using System;
using System.Text;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Controllers;
using SpectabisLib.Models;

namespace SpectabisUI.Controllers
{
    public class GameLibraryController : IGameLibraryController
    {
        private readonly IGameLauncher _gameLauncher;
        private readonly IDiscordService _discordService;
        private readonly IProfileRepository _profileRepository;

        public GameLibraryController(IGameLauncher gameLauncher, IDiscordService discordService, IProfileRepository profileRepository)
        {
            _gameLauncher = gameLauncher;
            _discordService = discordService;
            _profileRepository = profileRepository;
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
    }
}