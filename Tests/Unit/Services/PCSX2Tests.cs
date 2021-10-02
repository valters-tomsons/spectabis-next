using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SpectabisLib.Enums;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;
using SpectabisLib.Services;
using Xunit;
using Unit.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Configuration;

namespace Unit.Services
{
    public class PCSX2Tests
    {
        private readonly Mock<IProfileFileSystem> _pfsMock;
        private readonly Mock<IConfigurationManager> _configMock;

        private readonly GameLauncherPCSX2 _service;

        private readonly Guid FakeProfileGuid = new Guid("df690ae5-be5b-4b18-bcec-6ea121062a59");

        private const string EmulatorStubPath = "TestData/PCSX2Stub";
        private const string FakeProfilePath = "/spectabis/profiles/fakegameprofile";
        private const string FakeGamePath = "TestData/game.fake";

        public PCSX2Tests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _pfsMock = fixture.Freeze<Mock<IProfileFileSystem>>();
            _configMock = fixture.Freeze<Mock<IConfigurationManager>>();

            _service = fixture.Create<GameLauncherPCSX2>();
        }

        [UnixOnlyFact]
        public async Task EmulatorPathFromConfiguration_StartGameCalled_ReferencesCorrectFilesAndConfigArgument()
        {
            // Arrange
            var dirConf = new DirectoryConfig() { PCSX2Executable = new Uri($"{Environment.CurrentDirectory}/{EmulatorStubPath}") };
            _configMock.Setup(x => x.Directories).Returns(dirConf);
            _pfsMock.Setup(x => x.GetProfileContainerUriByType(It.IsAny<GameProfile>(), ContainerConfigType.Inis)).Returns(new Uri(FakeProfilePath));

            var game = new GameProfile()
            {
                Id = FakeProfileGuid,
                FilePath = FakeGamePath
            };

            // Act
            var actual = await _service.StartGame(game).ConfigureAwait(false);

            // Assert
            var stubOutput = await actual.GetProcess().StandardOutput.ReadToEndAsync().ConfigureAwait(false);

            Assert.Equal(FakeProfileGuid, actual.GetGame().Id);
            Assert.Equal(dirConf.PCSX2Executable.LocalPath, actual.GetProcess().StartInfo.FileName);
            Assert.NotNull(actual.GetProcess());

            Assert.Contains(game.FilePath, stubOutput);
            Assert.Contains($"--cfgpath={FakeProfilePath}", stubOutput);
        }

        [UnixOnlyFact]
        public async Task EmulatorPathProfileOverridden_StartGameCalled_ReferencesCorrectFilesAndConfigArgument()
        {
            // Arrange
            _pfsMock.Setup(x => x.GetProfileContainerUriByType(It.IsAny<GameProfile>(), ContainerConfigType.Inis)).Returns(new Uri(FakeProfilePath));

            var game = new GameProfile()
            {
                Id = FakeProfileGuid,
                FilePath = FakeGamePath,
                EmulatorPath = EmulatorStubPath
            };

            // Act
            var actual = await _service.StartGame(game).ConfigureAwait(false);

            // Assert
            var stubOutput = await actual.GetProcess().StandardOutput.ReadToEndAsync().ConfigureAwait(false);

            Assert.Equal(FakeProfileGuid, actual.GetGame().Id);
            Assert.Equal(game.EmulatorPath, actual.GetProcess().StartInfo.FileName);
            Assert.NotNull(actual.GetProcess());

            Assert.Contains(game.FilePath, stubOutput);
            Assert.Contains($"--cfgpath={FakeProfilePath}", stubOutput);
        }
    }
}