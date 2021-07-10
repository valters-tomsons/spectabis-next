using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;

namespace SpectabisLib.Repositories
{
    public class GameProfileRepository : IProfileRepository
    {
        private IList<GameProfile> _games;
        private readonly IProfileFileSystem _fileSystem;

        public GameProfileRepository(IProfileFileSystem pfs)
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

            await _fileSystem.WriteProfileToDisk(profile).ConfigureAwait(false);

            if(!_fileSystem.ProfileExistsOnFileSystem(profile))
            {
                await _fileSystem.WriteDefaultProfileToDisk(profile).ConfigureAwait(false);
            }
        }

        public GameProfile Get(Guid id)
        {
            if(_games.Count == 0)
            {
                throw new Exception("Games can only be queried when initialized from filesystem.");
            }

            return _games.SingleOrDefault(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<GameProfile>> GetAll()
        {
            return _games ??= await _fileSystem.GetAllProfilesFromDisk().ConfigureAwait(false);
        }

        public void DeleteProfile(GameProfile profile)
        {
            _games.Remove(profile);
            _fileSystem.DeleteProfileFromDisk(profile.Id);
        }
    }
}