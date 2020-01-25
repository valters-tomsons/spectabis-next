using System.Collections.Generic;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileRepository
    {
        IReadOnlyCollection<GameProfile> GetAll();
        void Add(GameProfile profile);
    }
}