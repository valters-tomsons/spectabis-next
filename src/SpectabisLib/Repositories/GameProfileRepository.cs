using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Services;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private IEnumerable<GameProfile> Games;
        private ProfileFileSystem _pfs { get; }

        public GameProfileRepository(ProfileFileSystem pfs)
        {
            _pfs = pfs;
        }

        public async Task UpsertProfile(GameProfile profile)
        {
            if(profile.Id == Guid.Empty)
            {
                profile.Id = Guid.NewGuid();
            }

            await _pfs.WriteProfileAsync(profile).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            await ReadProfiles().ConfigureAwait(false);
            return Games;
        }

        private async Task ReadProfiles()
        {
            if(Games != null)
            {
                return;
            }

            Games = await _pfs.ReadAllProfiles().ConfigureAwait(false);
        }
    }
}