using System.Threading;
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

        private static readonly SemaphoreSlim _filesystemSemaphore = new SemaphoreSlim(1);

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

            if(_games?.Contains(profile) == false)
            {
                _games.Add(profile);
            }

            await _fileSystem.CreateOnFileSystem(profile).ConfigureAwait(false);
            await _fileSystem.WriteDefaultConfiguration(profile).ConfigureAwait(false);
        }

        public async Task<GameProfile> Get(Guid id)
        {
            if(_games == null || _games.Count == 0)
            {
                await ReadFromDisk().ConfigureAwait(false);
            }

            return _games.SingleOrDefault(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<GameProfile>> ReadFromDisk()
        {
            await _filesystemSemaphore.WaitAsync().ConfigureAwait(false);
            _games ??= await _fileSystem.LoadFromFileSystem().ConfigureAwait(false);
            _filesystemSemaphore.Release();
            return _games;
        }

        public void DeleteProfile(GameProfile profile)
        {
            _games.Remove(profile);
            _fileSystem.DeleteFromFileSystem(profile.Id);
        }
    }
}