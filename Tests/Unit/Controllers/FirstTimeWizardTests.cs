using Xunit;
using Moq;
using SpectabisLib.Interfaces;
using SpectabisUI.Controllers;
using SpectabisLib.Configuration;

namespace Spectabis.Tests.Unit.Controllers
{
    public class FirstTimeWizardTests
    {
        private readonly Mock<IConfigurationManager> _configLoader = new Mock<IConfigurationManager>();

        [Fact]
        public void GivenTelemetryDisabled_GetTelemetryCalledWithToggle_ReturnsTelemetryEnabledMessage()
        {
            // Arrange
            var _config = new SpectabisConfig() { EnableTelemetry = false };

            _configLoader.SetupProperty(x => x.Spectabis, _config);
            _configLoader.SetupProperty(x => x.TextConfig, new TextConfig());

            _configLoader.Setup(x => x.WriteConfiguration(It.IsAny<SpectabisConfig>()));

            var expectedMessage = new TextConfig().TelemetryEnabled;
            var controller = new FirstTimeWizardController(_configLoader.Object);

            // Act
            var actual = controller.GetTelemetryStatusMessage(true);

            // Assert
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public void GivenTelemetryEnabled_GetTelemetryCalledWithToggle_ReturnsTelemetryDisabledMessage()
        {
            // Arrange
            var _config = new SpectabisConfig() { EnableTelemetry = true };

            _configLoader.SetupProperty(x => x.Spectabis, _config);
            _configLoader.SetupProperty(x => x.TextConfig, new TextConfig());
            _configLoader.Setup(x => x.WriteConfiguration(It.IsAny<SpectabisConfig>()));

            var expectedMessage = new TextConfig().TelemetryOptedOut;
            var controller = new FirstTimeWizardController(_configLoader.Object);

            // Act
            var actual = controller.GetTelemetryStatusMessage(true);

            // Assert
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public void GivenTelemetryEnabled_GetTelemetryCalledWithoutToggle_ReturnsTelemetryEnabled()
        {
            // Arrange
            var _config = new SpectabisConfig() { EnableTelemetry = true };

            _configLoader.SetupProperty(x => x.Spectabis, _config);
            _configLoader.SetupProperty(x => x.TextConfig, new TextConfig());
            _configLoader.Setup(x => x.WriteConfiguration(It.IsAny<SpectabisConfig>()));

            var expectedMessage = new TextConfig().TelemetryEnabled;
            var controller = new FirstTimeWizardController(_configLoader.Object);

            // Act
            var actual = controller.GetTelemetryStatusMessage(false);

            // Assert
            Assert.Equal(expectedMessage, actual);
        }

        [Fact]
        public void GetTelemetryCalledWithToggle_WriteConfigurationCalled()
        {
            // Arrange
            _configLoader.SetupProperty(x => x.Spectabis, new SpectabisConfig());
            _configLoader.SetupProperty(x => x.TextConfig, new TextConfig());
            _configLoader.Setup(x => x.WriteConfiguration(It.IsAny<SpectabisConfig>()));

            var controller = new FirstTimeWizardController(_configLoader.Object);

            // Act
            controller.GetTelemetryStatusMessage(true);

            // Assert
            _configLoader.Verify(x => x.WriteConfiguration(It.IsAny<SpectabisConfig>()), Times.Once);
        }
    }
}
