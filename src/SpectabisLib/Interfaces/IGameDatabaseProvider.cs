using SpectabisLib.Models;

namespace SpectabisLib.Interfaces
{
    public interface IGameDatabaseProvider
    {
        GameMetadata GetBySerial(string serial);
    }
}