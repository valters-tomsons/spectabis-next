using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Enums;
using SpectabisLib.Models;

namespace SpectabisLib.Interfaces.Abstractions
{
    public interface IProfileFileSystem
    {
        Task CopyDirectoryToGlobalProfile(Uri sourceDirectory, ContainerConfigType containerType);
        void DeleteFromFileSystem(Guid gameId);
        Task<IList<GameProfile>> LoadFromFileSystem();
        Uri? GameProfileArtUri(GameProfile profile);
        Uri GetProfileContainerUriByType(GameProfile profile, ContainerConfigType containerType);
        Task WriteProfileArtToFileSystem(GameProfile game, byte[] artBuffer);
        bool ProfileContainerHasAnyFiles(GameProfile profile, ContainerConfigType containerType);
        Task WriteDefaultConfiguration(GameProfile profile);
        Task CreateOnFileSystem(GameProfile profile);
    }
}