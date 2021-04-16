using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Models;

namespace SpectabisLib.Abstractions
{
    public class ProfileFileSystem
    {
        public async Task WriteProfileToDisk(GameProfile profile)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}", UriKind.Absolute);
            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = JsonConvert.SerializeObject(profile, Formatting.Indented);

            Directory.CreateDirectory(profileFolderUri.LocalPath);

            Logging.WriteLine($"Writing profile json '{profile.Id}'");
            await WriteStringToFile(profileUri, profileJson).ConfigureAwait(false);
        }

        public async Task WriteDefaultProfileToDisk(GameProfile profile)
        {
            var profileContainerUri = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}/container/inis/", UriKind.Absolute);
            Directory.CreateDirectory(profileContainerUri.LocalPath);

            Logging.WriteLine($"Writing default to profile container : `{profile.Id}`");

            var globalConfigUri = GetGlobalConfigsUri();
            var globalConfigFiles = Directory.EnumerateFiles(globalConfigUri.LocalPath, "*", SearchOption.TopDirectoryOnly).Select(Path.GetFileName);
            var globalConfigFiles2 = Directory.EnumerateFiles(globalConfigUri.LocalPath, "*", SearchOption.TopDirectoryOnly);

            foreach (var file in globalConfigFiles)
            {
                var fileSource = new Uri(globalConfigUri, file);
                var fileTarget = new Uri(profileContainerUri, file);

                using FileStream SourceStream = File.Open(fileSource.LocalPath, FileMode.Open);
                using FileStream DestinationStream = File.Create(fileTarget.LocalPath);
                await SourceStream.CopyToAsync(DestinationStream).ConfigureAwait(false);
            }
        }

        public async Task<IList<GameProfile>> GetAllProfilesFromDisk()
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

        public Uri GetBoxArtUri(GameProfile profile)
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

        public Uri GetContainerUri(GameProfile profile, ContainerConfigType containerType)
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

        public void DeleteProfileFromDisk(Guid gameId)
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

        public bool ProfileContainerExists(GameProfile profile)
        {
            var gameId = profile.Id;

            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{gameId}", UriKind.Absolute);
            var inisFolderUri = new Uri(profileFolderUri, "inis");

            return Directory.Exists(inisFolderUri.LocalPath);
        }

        public async Task SaveBoxArtToDisk(GameProfile game, byte[] artBuffer)
        {
            var artFilePath = new Uri($"{SystemDirectories.ProfileFolder}/{game.Id}/{Constants.BoxArtFileName}", UriKind.Absolute);
            await File.WriteAllBytesAsync(artFilePath.LocalPath, artBuffer).ConfigureAwait(false);
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
            return Encoding.Unicode.GetString(content);
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
            if(File.Exists(filePath.LocalPath))
            {
                Logging.WriteLine($"Deleting to overwrite'{filePath}'");
                File.Delete(filePath.LocalPath);
            }

            var encoded = Encoding.Unicode.GetBytes(text);

            using var stream = new FileStream(filePath.LocalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
            await stream.WriteAsync(encoded, 0, encoded.Length).ConfigureAwait(false);
        }

        private Uri GetGlobalConfigsUri()
        {
            return new Uri(SystemDirectories.GlobalConfigsFolder, UriKind.Absolute);
        }
    }
}