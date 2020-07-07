using System.Threading.Tasks;

namespace SpectabisService.Abstractions.Interfaces
{
    public interface IArtCacheProvider
    {
        Task<byte[]> GetFromCache(string serial);
        Task WriteToCache(string serial, byte[] image);
    }
}