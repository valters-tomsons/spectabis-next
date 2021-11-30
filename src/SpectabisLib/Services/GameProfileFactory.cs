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
            if(gameSerial is null)
            {
                return CreateProfileFromGameFilePath(gameFilePath);
            }

            var metadata = await _dbProvider.GetBySerial(gameSerial).ConfigureAwait(false);
            if(metadata is null)
            {
                return CreateProfileFromGameFilePath(gameFilePath);
            }

            return CreateProfileFromMetadata(gameFilePath, metadata);
        }

        private GameProfile CreateProfileFromGameFilePath(string gamePath)
        {
                var fileName = Path.GetFileName(gamePath);
                Logging.WriteLine($"Could not parse '{fileName}', using file name as game title");

                var title = Path.GetFileNameWithoutExtension(gamePath);

                return new GameProfile()
                {
                    FilePath = gamePath,
                    Title = title
                };
        }

        private GameProfile CreateProfileFromMetadata(string gamePath, GameMetadata metadata)
        {
            return new GameProfile()
            {
                FilePath = gamePath,
                SerialNumber = metadata.Serial,
                Title = metadata.Title
            };
        }
    }
}