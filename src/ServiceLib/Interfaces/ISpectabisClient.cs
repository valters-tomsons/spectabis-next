using System.Threading.Tasks;

namespace ServiceLib.Interfaces
{
    public interface ISpectabisClient
    {
        Task<byte[]> DownloadBoxArt(string serial);
    }
}