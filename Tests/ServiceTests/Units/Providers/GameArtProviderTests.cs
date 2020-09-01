using Moq;
using SpectabisLib.Interfaces;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Providers;
using SpectabisService.Providers.Interfaces;

namespace Tests.ServiceTests.Units.Providers
{
    public class GameArtProviderTests
    {
        private readonly IGameArtProvider artProvider;

        public GameArtProviderTests()
        {
            var artClient = new Mock<IGameArtClient>();
            var dbProvider = new Mock<IGameDatabaseProvider>();
            var storage = new Mock<IStorageProvider>();
        }

        public async void GivenInvalidSerial_ReturnsBadRequest()
        {

        }
    }
}