using System.IO;
using System.Threading.Tasks;
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

        async Task<GameProfile> IProfileFactory.CreateFromPath(string gameFilePath)
        {
            if(!File.Exists(gameFilePath))
            {
                throw new FileNotFoundException(gameFilePath);
            }

            var gameSerial = await _gameParser.GetGameSerial(gameFilePath).ConfigureAwait(false);
            var metadata = _dbProvider.GetBySerial(gameSerial);

            if(metadata == null)
            {
                var fileName = Path.GetFileName(gameFilePath);
                System.Console.WriteLine($"[GameProfileFactory] Could not parse '{fileName}', using file name as game title");

                return new GameProfile()
                {
                    FilePath = gameFilePath,
                    Title = GetNameFromPath(gameFilePath)
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