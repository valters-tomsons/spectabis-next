using SpectabisLib.Helpers;
using SpectabisLib.Interfaces.Services;

namespace SpectabisLib.Services
{
    public class GameConfigurationService : IGameConfigurationService
    {
        public void Test()
        {
            Logging.WriteLine("Printer from config service");
        }
    }
}