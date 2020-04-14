using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisLib.Services;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private IList<GameProfile> Games;
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

            if(!Games.Contains(profile))
            {
                Games.Add(profile);
            }

            await _pfs.WriteProfileAsync(profile).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            await ReadProfiles().ConfigureAwait(false);
            return Games;
        }

        public void DeleteProfile(GameProfile profile)
        {
            Games.Remove(profile);
            _pfs.DeleteProfileAsync(profile.Id);
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