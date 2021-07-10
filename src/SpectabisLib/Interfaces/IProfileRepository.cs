using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileRepository
    {
        Task UpsertProfile(GameProfile profile);
        Task<IEnumerable<GameProfile>> ReadFromDisk();
        void DeleteProfile(GameProfile profile);
        Task<GameProfile> Get(Guid id);
    }
}