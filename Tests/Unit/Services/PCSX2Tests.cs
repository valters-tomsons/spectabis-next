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

namespace Unit.Services
{
    public class PCSX2Tests
    {
        private readonly IProfileFileSystem _pfsMock;

        private readonly GameLauncherPCSX2 _service;

        public PCSX2Tests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _pfsMock = fixture.Freeze<IProfileFileSystem>();

            _service = fixture.Create<GameLauncherPCSX2>();
        }

        [UnixOnlyFact]
        public async Task GivenProfileValid_StartGameCalled_ReferencesCorrectFilesAndConfigArgument()
        {
            Mock.Get(_pfsMock).Setup(x => x.GetContainerUri(It.IsAny<GameProfile>(), ContainerConfigType.Inis)).Returns(new Uri("/"));

            var game = new GameProfile()
            {
                Id = Guid.NewGuid(),
                EmulatorPath = "TestData/PCSX2Stub",
                FilePath = "TestData/game.fake",
            };

            var actual = await _service.StartGame(game).ConfigureAwait(false);
            var stubOutput = await actual.GetProcess().StandardOutput.ReadToEndAsync().ConfigureAwait(false);

            Assert.Equal(game.Id, actual.GetGame().Id);
            Assert.Equal(game.EmulatorPath, actual.GetProcess().StartInfo.FileName);
            Assert.NotNull(actual.GetProcess());

            Assert.Contains(game.FilePath, stubOutput);
            Assert.Contains("--cfgpath=", stubOutput);
        }
    }
}