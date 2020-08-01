using System.Linq;
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
        private IList<GameProfile> _games;
        private ProfileFileSystem _fileSystem { get; }

        public GameProfileRepository(ProfileFileSystem pfs)
        {
            _fileSystem = pfs;
        }

        public async Task UpsertProfile(GameProfile profile)
        {
            if(profile.Id == Guid.Empty)
            {
                profile.Id = Guid.NewGuid();
            }

            if(!_games.Contains(profile))
            {
                _games.Add(profile);
            }

            await _fileSystem.WriteProfileAsync(profile).ConfigureAwait(false);

            if(!_fileSystem.IsProfileContainerValid(profile))
            {
                await CopyDefault(profile).ConfigureAwait(false);
            }
        }

        public GameProfile Get(Guid gameId)
        {
            return _games.FirstOrDefault(x => x.Id == gameId);
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            await ReadProfiles().ConfigureAwait(false);
            return _games;
        }

        public void DeleteProfile(GameProfile profile)
        {
            _games.Remove(profile);
            _fileSystem.DeleteProfile(profile.Id);
        }

        private async Task CopyDefault(GameProfile profile)
        {
            await _fileSystem.CopyDefaultConfiguration(profile).ConfigureAwait(false);
        }

        private async Task ReadProfiles()
        {
            if(_games != null)
            {
                return;
            }

            _games = await _fileSystem.ReadAllProfiles().ConfigureAwait(false);
        }
    }
}