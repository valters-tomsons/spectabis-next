using System;
using System.Threading.Tasks;
using EmuConfig.Interfaces;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Services
{
    public interface IGameConfigurationService
    {
        Task<ProfileConfiguration> Get(Guid gameId);
        Task UpdateConfiguration<T>(Guid gameId, T config) where T : IConfigurable;
    }
}