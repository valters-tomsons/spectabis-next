using System.IO;
using System.Threading.Tasks;
using FileIntrinsics;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameProfileFactory : IProfileFactory
    {
        public GameProfileFactory()
        {
        }

        async Task<GameProfile> IProfileFactory.CreateFromPath(string gameFilePath)
        {
            if(!File.Exists(gameFilePath))
            {
                throw new FileNotFoundException(gameFilePath);
            }

            var fileId = new IntrinsicsProvider();
            var isIsoTask = fileId.SignatureFound(gameFilePath, FileIntrinsics.Signatures.ISO9660.Signature);

            var profile = new GameProfile()
            {
                FilePath = gameFilePath
            };

            var isIso = await isIsoTask;
            System.Console.WriteLine($"File is iso");

            return profile;
        }
    }
}