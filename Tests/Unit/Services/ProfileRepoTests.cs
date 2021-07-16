using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;
using SpectabisLib.Repositories;
using Xunit;

namespace Unit.Services
{
    public class ProfileRepoTests
    {
        private readonly IProfileFileSystem _fileSystemMock;
        private readonly GameProfileRepository _service;

        public ProfileRepoTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fileSystemMock = fixture.Freeze<IProfileFileSystem>();

            _service = fixture.Create<GameProfileRepository>();
        }

        [Fact]
        public async Task GivenEmptyGuid_UpsertProfileCalled_GuidAssigned()
        {
            var profile = new GameProfile();

            await _service.UpsertProfile(profile);

            Assert.NotEqual(Guid.Empty, profile.Id);
        }

        [Fact]
        public async Task GivenProfile_UpsertProfileCalled_FileSystemCalled()
        {
            var profile = new GameProfile();

            await _service.UpsertProfile(profile);

            Mock.Get(_fileSystemMock).Verify(x => x.WriteProfileToDisk(profile), Times.Once);
            Mock.Get(_fileSystemMock).Verify(x => x.WriteDefaultProfileToDisk(profile), Times.Once);
        }

        [Fact]
        public async Task GivenNoGamesInCache_GetByIdCalled_ReturnedFromFileSystem()
        {
            var profile = new GameProfile() { Id = Guid.NewGuid() };
            Mock.Get(_fileSystemMock).Setup(x => x.GetAllProfilesFromDisk()).ReturnsAsync(new List<GameProfile> { profile });

            var actual = await _service.Get(profile.Id);

            Assert.Equal(profile, actual);
            Mock.Get(_fileSystemMock).Verify(x => x.GetAllProfilesFromDisk(), Times.Once);
        }

        [Fact]
        public async Task GivenCache_GetByIdCalledTwice_FileSystemCalledOnlyOnce()
        {
            var profile = new GameProfile() { Id = Guid.NewGuid() };
            Mock.Get(_fileSystemMock).Setup(x => x.GetAllProfilesFromDisk()).ReturnsAsync(new List<GameProfile> { profile });

            var actual1 = await _service.Get(profile.Id);
            var actual2 = await _service.Get(profile.Id);

            Assert.Equal(profile, actual1);
            Assert.Equal(profile, actual2);
            Mock.Get(_fileSystemMock).Verify(x => x.GetAllProfilesFromDisk(), Times.Once);
        }
    }
}