using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SpectabisLib.Helpers;
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

            var isoReader = new ISO9660.IsoReader();
            var isIsoTask = isoReader.IsIso9660(gameFilePath);

            var profile = new GameProfile()
            {
                FilePath = gameFilePath
            };

            var isIso = await isIsoTask;
            System.Console.WriteLine(isIso);

            return profile;
        }
    }
}