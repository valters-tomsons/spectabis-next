using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameDatabaseProvider
    {
        Task<GameMetadata> GetBySerial(string serial);
        Task<GameMetadata> GetNearestByTitle(string title);
        Task<IEnumerable<GameMetadata>> QueryByTitle(string title, int count = 5);
        Task IntializeDatabase();
    }
}