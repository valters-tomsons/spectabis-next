using System.IO;
using System.Threading.Tasks;
using Common.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameProfileFactory : IProfileFactory
    {
        private readonly IGameFileParser _gameParser;
        private readonly IGameDatabaseProvider _dbProvider;

        public GameProfileFactory(IGameFileParser gameParser, IGameDatabaseProvider dbProvider)
        {
            _gameParser = gameParser;
            _dbProvider = dbProvider;
        }

        async Task<GameProfile?> IProfileFactory.CreateFromPath(string gameFilePath)
        {
            if(!File.Exists(gameFilePath))
            {
                throw new FileNotFoundException(gameFilePath);
            }

            var gameSerial = await _gameParser.GetGameSerial(gameFilePath).ConfigureAwait(false);

            if(string.IsNullOrWhiteSpace(gameSerial))
            {
                return null;
            }

            var metadata = await _dbProvider.GetBySerial(gameSerial).ConfigureAwait(false);

            if(metadata is null)
            {
                var fileName = Path.GetFileName(gameFilePath);
                Logging.WriteLine($"Could not parse '{fileName}', using file name as game title");

                var title = GetNameFromPath(gameFilePath);

                return new GameProfile()
                {
                    FilePath = gameFilePath,
                    Title = title
                };
            }

            return new GameProfile()
            {
                SerialNumber = gameSerial,
                FilePath = gameFilePath,
                Title = metadata.Title
            };
        }

        private string GetNameFromPath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}