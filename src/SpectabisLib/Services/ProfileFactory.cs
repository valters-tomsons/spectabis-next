using System;
using System.Collections.Generic;
using SpectabisLib.Helpers;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class ProfileFactory
    {
        private SystemPathsStruct _systemPaths { get; set; }

        public ProfileFactory(SystemPathsStruct systemPaths)
        {
            _systemPaths = systemPaths;
        }

        public IEnumerable<GameProfile> ReadAllProfiles()
        {
            throw new NotImplementedException();
        }

        public GameProfile WriteProfile(GameProfile profile)
        {
            throw new NotImplementedException();
        }

        public GameProfile ReadProfileById(string profileId)
        {
            var profileDirectory = $"{_systemPaths.ProfilesFolder}/{profileId}";

            throw new NotImplementedException();
        }

        private GameProfile GenerateProfile(GameProfile profileProperties)
        {
            // var profile = 
            throw new NotImplementedException();
        }
    }
}