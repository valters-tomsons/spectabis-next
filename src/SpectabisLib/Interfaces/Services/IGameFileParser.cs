using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface IGameFileParser
    {
        Task<string> GetGameSerial(string filePath);
    }
}