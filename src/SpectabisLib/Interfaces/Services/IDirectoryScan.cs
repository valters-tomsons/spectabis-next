using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface IDirectoryScan
    {
        Task<IEnumerable<string>> ScanNewGames();
    }
}