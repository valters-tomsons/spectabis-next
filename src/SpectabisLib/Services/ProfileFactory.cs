using System;
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

        public GameProfile WriteProfile()
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