using System;
using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface IGameArtClient
    {
        Task<Uri?> GetBoxArtPS2(string titleQuery);
    }
}