using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameDatabaseProvider
    {
        Task<GameMetadata> GetBySerial(string serial);
        Task<GameMetadata> GetNearestByTitle(string title);
    }
}