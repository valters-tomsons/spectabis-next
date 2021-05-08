using System.Threading.Tasks;

namespace SpectabisLib.Interfaces.Services
{
    public interface ILocalCachingService
    {
        Task<byte[]> GetCachedArt(string gameSerial);
        Task WriteArtToCache(string gameSerial, byte[] buffer);
    }
}