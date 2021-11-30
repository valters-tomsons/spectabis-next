using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileFactory
    {
        Task<GameProfile?> CreateFromPath(string gameFilePath);
    }
}