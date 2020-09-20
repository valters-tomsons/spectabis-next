using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpectabisLib.Interfaces
{
    public interface ISettingsController
    {
        Task<IEnumerable<string>> AppendScanDirectory(string updated);
        Task<IEnumerable<string>> RemoveScanDirectory(string target);
        Task UpdateOptions(bool telemetry, bool discord);
    }
}