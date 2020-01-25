using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
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

        public GameProfile CreateFromPath(string filePath)
        {
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            var profile = new GameProfile()
            {
                FilePath = filePath
            };

            return profile;
        }
    }
}