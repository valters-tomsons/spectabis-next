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
        private IList<GameProfile> Games { get; set; } = Array.Empty<GameProfile>();
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

            if(Games?.Contains(profile) == false)
            {
                Games.Add(profile);
            }

            await _fileSystem.CreateOnFileSystem(profile).ConfigureAwait(false);
            await _fileSystem.WriteDefaultConfiguration(profile).ConfigureAwait(false);
        }

        public async Task<GameProfile> Get(Guid id)
        {
            if(Games == null || Games.Count == 0)
            {
                await ReadFromDisk().ConfigureAwait(false);
            }

            return Games.SingleOrDefault(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<GameProfile>> ReadFromDisk()
        {
            await _filesystemSemaphore.WaitAsync().ConfigureAwait(false);
            Games ??= await _fileSystem.LoadFromFileSystem().ConfigureAwait(false);
            _filesystemSemaphore.Release();
            return Games;
        }

        public void DeleteProfile(GameProfile profile)
        {
            Games.Remove(profile);
            _fileSystem.DeleteFromFileSystem(profile.Id);
        }
    }
}