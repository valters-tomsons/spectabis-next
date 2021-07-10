using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Enums;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Abstractions
{
    public interface IProfileFileSystem
    {
        Task CopyToGlobalContainer(Uri sourceDirectory, ContainerConfigType containerType);
        void DeleteProfileFromDisk(Guid gameId);
        Task<IList<GameProfile>> GetAllProfilesFromDisk();
        Uri GetBoxArtUri(GameProfile profile);
        Uri GetContainerUri(GameProfile profile, ContainerConfigType containerType);
        bool ProfileExistsOnFileSystem(GameProfile profile);
        Task SaveBoxArtToDisk(GameProfile game, byte[] artBuffer);
        bool ValidateContainerContent(GameProfile profile, ContainerConfigType containerType);
        Task WriteDefaultProfileToDisk(GameProfile profile);
        Task WriteProfileToDisk(GameProfile profile);
    }
}