using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileRepository
    {
        Task UpsertProfile(GameProfile profile);
        Task<IEnumerable<GameProfile>> GetAll();
        void DeleteProfile(GameProfile profile);
    }
}