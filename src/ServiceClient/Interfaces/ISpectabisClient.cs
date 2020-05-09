using System;
using System.Threading.Tasks;

namespace ServiceClient.Interfaces
{
    public interface ISpectabisClient
    {
        Task<Uri> GetBoxArtUrl(string serial);
    }
}