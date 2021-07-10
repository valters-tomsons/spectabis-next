using System;
using System.Threading.Tasks;
using EmuConfig.Configs;
using EmuConfig.Interfaces;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Interfaces.Services;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class GameConfigurationService : IGameConfigurationService
    {
        private readonly IProfileRepository _profileRepo;
        private readonly IParserProvider _iniParser;
        private readonly IProfileFileSystem _fileSystem;

        public GameConfigurationService(IProfileRepository profileRepo, IProfileFileSystem fileSystem, IParserProvider iniParser)
        {
            _profileRepo = profileRepo;
            _iniParser = iniParser;
            _fileSystem = fileSystem;
        }

        public async Task<ProfileConfiguration> Get(Guid gameId)
        {
            var profile = await _profileRepo.Get(gameId).ConfigureAwait(false);

            var containerUri = _fileSystem.GetContainerUri(profile, Enums.ContainerConfigType.Inis);
            containerUri = new Uri(containerUri, "GSdx.ini");

            var gsConfig = _iniParser.ReadConfig<GSdxConfig>(containerUri);
            return new ProfileConfiguration() { GSdxConfig = gsConfig };
        }

        public async Task UpdateConfiguration<T>(Guid gameId, T config) where T : IConfigurable
        {
            var profile = await _profileRepo.Get(gameId).ConfigureAwait(false);

            var containerUri = _fileSystem.GetContainerUri(profile, Enums.ContainerConfigType.Inis);
            containerUri = new Uri(containerUri, "GSdx.ini");

            Logging.WriteLine($"Writing `{gameId}` configuration to `{containerUri.LocalPath}`");
            await _iniParser.WriteConfig(containerUri, config).ConfigureAwait(false);
        }
    }
}