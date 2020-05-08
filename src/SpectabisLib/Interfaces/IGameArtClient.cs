using System;
using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface IGameArtClient
    {
        Task<Uri> GetBoxArt(string titleQuery);
    }
}