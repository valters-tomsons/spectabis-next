using System;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;
using SpectabisLib.Services;
using Xunit;

namespace Unit.Services
{
    public class PCSX2Tests
    {
        private readonly GameLauncherPCSX2 _service;

        public PCSX2Tests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _service = fixture.Create<GameLauncherPCSX2>();
        }

        [Fact]
        public async Task Test()
        {
            var game = new GameProfile() { Id = Guid.NewGuid() };

            var actual = await _service.StartGame(game);
            actual.GetProcess().Kill(true);

            Assert.Equal(game.Id, actual.GetGame().Id);
            Assert.NotNull(actual.GetProcess());
        }
    }
}