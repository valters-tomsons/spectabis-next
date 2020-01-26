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
            var iscueTask = fileId.SignatureFound(gameFilePath, FileIntrinsics.Signatures.CueDescription.Signature);

            var profile = new GameProfile()
            {
                FilePath = gameFilePath
            };

            var isIso = await isIsoTask;
            var isCue = await iscueTask;

            System.Console.WriteLine($"isIso: {isIso}");
            System.Console.WriteLine($"isCue: {isCue}");

            return profile;
        }
    }
}