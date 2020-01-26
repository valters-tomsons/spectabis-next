using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IProfileFactory
    {
        Task<GameProfile> CreateFromPath(string gameFilePath);
        // 7FF8 - god hand
        // 7FF8 - mgs3-1
        // 7FF8 - mgs3-2
    }
}