using System.IO;
using System.Threading.Tasks;
using FileIntrinsics;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameProfileFactory : IProfileFactory
    {
        private readonly IGameFileParser _gameParser;

        public GameProfileFactory(IGameFileParser gameParser)
        {
            _gameParser = gameParser;
        }

        async Task<GameProfile> IProfileFactory.CreateFromPath(string gameFilePath)
        {
            if(!File.Exists(gameFilePath))
            {
                throw new FileNotFoundException(gameFilePath);
            }

            var gameSerial = await _gameParser.GetGameSerial(gameFilePath).ConfigureAwait(false);

            var profile = new GameProfile()
            {
                SerialNumber = gameSerial,
                FilePath = gameFilePath
            };

            return profile;
        }
    }
}