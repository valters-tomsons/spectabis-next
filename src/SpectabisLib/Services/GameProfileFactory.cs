using System;
using System.Collections.Generic;
using System.IO;
using SpectabisLib.Helpers;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameProfileFactory
    {
        private SystemPathsStruct _systemPaths { get; set; }
        private static Random Random = new Random();

        public GameProfileFactory(SystemPathsStruct systemPaths)
        {
            _systemPaths = systemPaths;
        }

        public GameProfile CreateFromPath(string filePath, string gameName)
        {
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var profile = new GameProfile()
            {
                Id = GetNewId(),
                Title = gameName,
                FilePath = filePath
            };

            return profile;
        }

        private string GetNewId()
        {
            var guid = new Guid();
            return guid.ToString();
        }
    }
}