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

namespace SpectabisLib.Services
{
    public class ProfileFileSystem
    {
        public Uri GetProfileConfigLocation(GameProfile profile, ContainerConfigType containerType)
        {
            if(profile.Id == Guid.Empty)
            {
                throw new Exception("Game guid is empty");
            }

            var containerDirectoryName = ContainerConfigTypeParser.GetTypeDirectoryName(containerType);
            var location = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}/container/{containerDirectoryName}", UriKind.Absolute);

            Directory.CreateDirectory(location.LocalPath);
            return location;
        }

        public async Task WriteProfileAsync(GameProfile profile)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}", UriKind.Absolute);
            Directory.CreateDirectory(profileFolderUri.LocalPath);

            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = JsonConvert.SerializeObject(profile, Formatting.Indented);

            Console.WriteLine($"[ProfileFileSystem] Writing profile json '{profile.Id}'");

            await WriteTextAsync(profileUri, profileJson).ConfigureAwait(false);
        }

        public async Task<IList<GameProfile>> ReadAllProfiles()
        {
            var guids = GetAllProfileIds();
            var profiles = new List<GameProfile>();

            foreach (var item in guids)
            {
                var game = await ReadProfileAsync(item).ConfigureAwait(false);
                profiles.Add(game);
            }

            return profiles;
        }

        public async Task<GameProfile> ReadProfileAsync(Guid gameId)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{gameId}", UriKind.Absolute);

            if (!Directory.Exists(profileFolderUri.LocalPath))
            {
                Console.WriteLine($"[ProfileFileSystem] Profile '{gameId}' does not exist");
                return null;
            }

            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = await ReadTextAsync(profileUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<GameProfile>(profileJson);
        }

        public void DeleteProfile(Guid gameId)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{gameId}", UriKind.Absolute);
            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);

            if (!Directory.Exists(profileFolderUri.LocalPath))
            {
                Console.WriteLine($"[ProfileFileSystem] Profile folder '{gameId}' does not exist");
                return;
            }

            if(!File.Exists(profileUri.LocalPath))
            {
                Console.WriteLine($"[ProfileFileSystem] Profile json '{gameId}' does not exist");
                return;
            }

            Directory.Delete(profileFolderUri.LocalPath, true);
            Console.WriteLine($"[ProfileFileSystem] Profile '{gameId}' deleted");
        }

        private IEnumerable<Guid> GetAllProfileIds()
        {
            var profilesFolderUri = new Uri(SystemDirectories.ProfileFolder, UriKind.Absolute);
            var directories = Directory.GetDirectories(profilesFolderUri.LocalPath).Select(Path.GetFileName);

            var guids = new List<Guid>();

            foreach (var item in directories)
            {
                var guid = Guid.TryParse(item, out Guid result);
                if(guid)
                {
                    guids.Add(result);
                }
            }

            return guids;
        }

        private async Task<string> ReadTextAsync(Uri filePath)
        {
            using(var stream = File.OpenRead(filePath.LocalPath))
            {
                var content = new byte[stream.Length];
                await stream.ReadAsync(content, 0, (int) stream.Length).ConfigureAwait(false);
                return Encoding.Unicode.GetString(content);
            }
        }

        private async Task WriteTextAsync(Uri filePath, string text)
        {
            var encoded = Encoding.Unicode.GetBytes(text);

            using(var stream = new FileStream(filePath.LocalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize : 4096, useAsync : true))
            {
                await stream.WriteAsync(encoded, 0, encoded.Length).ConfigureAwait(false);
            }
        }
    }
}