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
        private ProfileFileSystem Pfs { get; }

        public GameProfileRepository(ProfileFileSystem pfs)
        {
            Pfs = pfs;
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

            await Pfs.WriteProfileAsync(profile).ConfigureAwait(false);

            if(!Pfs.IsProfileContainerValid(profile))
            {
                await CopyDefault(profile).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            await ReadProfiles().ConfigureAwait(false);
            return Games;
        }

        public void DeleteProfile(GameProfile profile)
        {
            Games.Remove(profile);
            Pfs.DeleteProfile(profile.Id);
        }

        private async Task CopyDefault(GameProfile profile)
        {
            await Pfs.CopyDefaultConfiguration(profile).ConfigureAwait(false);
        }

        private async Task ReadProfiles()
        {
            if(Games != null)
            {
                return;
            }

            Games = await Pfs.ReadAllProfiles().ConfigureAwait(false);
        }
    }
}