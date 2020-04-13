using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileRepository
    {
        IReadOnlyCollection<GameProfile> GetAll();
        void Add(GameProfile profile);
        Task UpsertProfile(GameProfile profile);
    }
}