using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpectabisService.Providers.Interfaces
{
    public interface IGameArtProvider
    {
        Task<IActionResult> GetArtWithSerial(string serial);
    }
}