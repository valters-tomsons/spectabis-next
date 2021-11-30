using System;
using System.Threading.Tasks;

namespace SpectabisService.Services.Interfaces
{
    public interface IContentDownloader
    {
        Task<byte[]?> DownloadGameArt(Uri imageUri);
    }
}