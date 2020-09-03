using System.Threading.Tasks;

namespace ServiceClient.Interfaces
{
    public interface ISpectabisClient
    {
        Task<byte[]> DownloadBoxArt(string serial);
    }
}