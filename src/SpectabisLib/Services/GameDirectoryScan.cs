using System.IO;
using System.Collections.Generic;
using SpectabisLib.Interfaces;
using FileIntrinsics.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Common.Helpers;
using System;

namespace SpectabisLib.Services
{
    public class GameDirectoryScan : IDirectoryScan
    {
        private readonly IProfileRepository _gameRepo;
        private readonly IConfigurationManager _configuration;

        private IEnumerable<string> Extensions { get; }

        public GameDirectoryScan(IConfigurationManager config, IIntrinsicsProvider fileIntrinsics, IProfileRepository gameRepo)
        {
            _gameRepo = gameRepo;
            _configuration = config;

            Extensions = fileIntrinsics.GetKnownExtensions();
        }

        public async Task <IEnumerable<string>> ScanNewGames()
        {
            var _directories = _configuration.Directories.GameScanDirectories;

            if(!_directories.Any())
            {
                return new List<string>();
            }

            var allGamesTask = _gameRepo.ReadFromDisk();
            var foundFiles = FindGameFiles(_directories, Extensions);

            var loadedGames = await allGamesTask.ConfigureAwait(false);
            var loadedPaths = loadedGames.Select(x => x.FilePath);

            var newGames = new List<string>();

            foreach(var file in foundFiles)
            {
                var alreadyLoaded = loadedPaths.Any(x => x == file);

                if(!alreadyLoaded)
                {
                    newGames.Add(file);
                }
            }

            return newGames;
        }

        private IEnumerable<string> FindGameFiles(IEnumerable<string> directories, IEnumerable<string> extensions)
        {
            var files = new List<string>();

            foreach (var dir in directories)
            {
                try
                {
                    var dirFiles = Directory.EnumerateFiles(dir, string.Empty, SearchOption.AllDirectories);
                    files.AddRange(dirFiles);
                }
                catch(Exception e)
                {
                    Logging.WriteLine($"Exception when trying to enumerate directory `{dir}`");
                    Logging.WriteLine($"Exception: {e.Message}");
                }
            }

            var matches = new List<string>();

            foreach (var file in files)
            {
                foreach (var extension in extensions)
                {
                    if(file.EndsWith(extension))
                    {
                        matches.Add(file);
                    }
                }
            }

            return matches;
        }
    }
}