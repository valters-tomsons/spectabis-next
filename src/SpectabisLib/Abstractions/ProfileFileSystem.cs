using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Common.Helpers;
using SpectabisLib.Helpers;
using SpectabisLib.Enums;
using SpectabisLib.Interfaces.Abstractions;
using SpectabisLib.Models;

namespace SpectabisLib.Abstractions
{
    public class ProfileFileSystem : IProfileFileSystem
    {
        public async Task CreateOnFileSystem(GameProfile profile)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}", UriKind.Absolute);
            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = JsonConvert.SerializeObject(profile, Formatting.Indented);

            Directory.CreateDirectory(profileFolderUri.LocalPath);

            Logging.WriteLine($"Writing profile json '{profile.Id}'");
            await WriteStringToFile(profileUri, profileJson).ConfigureAwait(false);
        }

        public async Task WriteDefaultConfiguration(GameProfile profile)
        {
            var profileContainerUri = GetProfileContainerUriByType(profile, ContainerConfigType.Inis);
            Directory.CreateDirectory(profileContainerUri.LocalPath);

            Logging.WriteLine($"Writing default to profile container : `{profile.Id}`");

            var globalContainerPath = GetGlobalConfigsUri();
            var sourcePath = new Uri(globalContainerPath, "inis/");

            var globalConfigFiles = Directory.EnumerateFiles(sourcePath.LocalPath, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);

            foreach (var file in globalConfigFiles)
            {
                var fileSource = new Uri(sourcePath, file);
                var fileTarget = new Uri(profileContainerUri, file);

                using FileStream SourceStream = File.Open(fileSource.LocalPath, FileMode.Open);
                using FileStream DestinationStream = File.Create(fileTarget.LocalPath);
                await SourceStream.CopyToAsync(DestinationStream).ConfigureAwait(false);
            }
        }

        public async Task<IList<GameProfile>> LoadFromFileSystem()
        {
            var guids = EnumerateDiskProfileIDs();
            var profiles = new List<GameProfile>();

            foreach (var item in guids)
            {
                var game = await ReadProfileFromDisk(item).ConfigureAwait(false);
                profiles.Add(game);
            }

            return profiles;
        }

        public Uri GameProfileArtUri(GameProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.BoxArtPath))
            {
                var artFilePath = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}/{Constants.BoxArtFileName}", UriKind.Absolute);

                if (File.Exists(artFilePath.LocalPath))
                {
                    return artFilePath;
                }

                return null;
            }

            return new Uri(profile.BoxArtPath, UriKind.Absolute);
        }

        public Uri GetProfileContainerUriByType(GameProfile profile, ContainerConfigType containerType)
        {
            if (profile.Id == Guid.Empty)
            {
                throw new Exception("Game guid is empty");
            }

            var containerDirectoryName = ContainerConfigTypeParser.GetTypeDirectoryName(containerType);
            var location = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}/container/{containerDirectoryName}/", UriKind.Absolute);

            Directory.CreateDirectory(location.LocalPath);
            return location;
        }

        /// <summary>
        /// Returns `true` if provided profile container has any configuration files.
        /// </summary>
        public bool ProfileContainerHasAnyFiles(GameProfile profile, ContainerConfigType containerType)
        {
            if (profile.Id == Guid.Empty)
            {
                throw new Exception("Game guid is empty");
            }

            var containerUri = GetProfileContainerUriByType(profile, containerType);

            var files = Directory.EnumerateFiles(containerUri.LocalPath);
            return files.Any();
        }

        public void DeleteFromFileSystem(Guid gameId)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{gameId}", UriKind.Absolute);
            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);

            if (!Directory.Exists(profileFolderUri.LocalPath))
            {
                Logging.WriteLine($"Profile folder '{gameId}' does not exist");
                return;
            }

            if (!File.Exists(profileUri.LocalPath))
            {
                Logging.WriteLine($"Profile json '{gameId}' does not exist");
                return;
            }

            Directory.Delete(profileFolderUri.LocalPath, true);
            Logging.WriteLine($"Profile '{gameId}' deleted");
        }

        public async Task WriteProfileArtToFileSystem(GameProfile game, byte[] artBuffer)
        {
            if (!Directory.Exists($"{SystemDirectories.ProfileFolder}/{game.Id}"))
            {
                Logging.WriteLine("Profile does not exist anymore, discarding game art...");
                return;
            }

            var artFilePath = new Uri($"{SystemDirectories.ProfileFolder}/{game.Id}/{Constants.BoxArtFileName}", UriKind.Absolute);
            await File.WriteAllBytesAsync(artFilePath.LocalPath, artBuffer).ConfigureAwait(false);
        }

        public async Task CopyDirectoryToGlobalProfile(Uri sourceDirectory, ContainerConfigType containerType)
        {
            if (!Directory.Exists(sourceDirectory.LocalPath))
            {
                Logging.WriteLine($"Source directory '{sourceDirectory.LocalPath}' does not exist, not copying to global container");
                return;
            }

            var globalContainerPath = GetGlobalConfigsUri();
            var targetContainerName = ContainerConfigTypeParser.GetTypeDirectoryName(containerType) + "/";
            var targetPath = new Uri(globalContainerPath, targetContainerName);

            var sourceFiles = Directory.EnumerateFiles(sourceDirectory.LocalPath, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);

            Directory.CreateDirectory(targetPath.LocalPath);

            foreach (var file in sourceFiles)
            {
                var fileSource = new Uri(sourceDirectory, file);
                var fileTarget = new Uri(targetPath, file);

                using FileStream SourceStream = File.Open(fileSource.LocalPath, FileMode.Open);
                using FileStream DestinationStream = File.Create(fileTarget.LocalPath);
                await SourceStream.CopyToAsync(DestinationStream).ConfigureAwait(false);
            }
        }

        private IEnumerable<Guid> EnumerateDiskProfileIDs()
        {
            var profilesFolderUri = new Uri(SystemDirectories.ProfileFolder, UriKind.Absolute);
            var directories = Directory.GetDirectories(profilesFolderUri.LocalPath).Select(Path.GetFileName);

            var guids = new List<Guid>();

            foreach (var item in directories)
            {
                var guid = Guid.TryParse(item, out Guid result);
                if (guid)
                {
                    guids.Add(result);
                }
            }

            return guids;
        }

        private async Task<string> ReadFileAsString(Uri filePath)
        {
            using var stream = File.OpenRead(filePath.LocalPath);
            var content = new byte[stream.Length];
            await stream.ReadAsync(content, 0, (int)stream.Length).ConfigureAwait(false);
            return Encoding.UTF8.GetString(content);
        }

        private async Task<GameProfile> ReadProfileFromDisk(Guid gameId)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{gameId}", UriKind.Absolute);

            if (!Directory.Exists(profileFolderUri.LocalPath))
            {
                Logging.WriteLine($"Profile '{gameId}' does not exist");
                return null;
            }

            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = await ReadFileAsString(profileUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<GameProfile>(profileJson);
        }

        private async Task WriteStringToFile(Uri filePath, string text)
        {
            if (File.Exists(filePath.LocalPath))
            {
                Logging.WriteLine($"Deleting to overwrite'{filePath}'");
                File.Delete(filePath.LocalPath);
            }

            var encoded = Encoding.UTF8.GetBytes(text);

            using var stream = new FileStream(filePath.LocalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await stream.WriteAsync(encoded, 0, encoded.Length).ConfigureAwait(false);
        }

        private Uri GetGlobalConfigsUri()
        {
            return new Uri(SystemDirectories.GlobalConfigsFolder, UriKind.Absolute);
        }
    }
}