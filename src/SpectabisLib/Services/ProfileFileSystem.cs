using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Helpers;
using SpectabisLib.Models;

namespace SpectabisLib.Services
{
    public class ProfileFileSystem
    {
        public async Task WriteProfileAsync(GameProfile profile)
        {
            var profileFolderUri = new Uri($"{SystemDirectories.ProfileFolder}/{profile.Id}", UriKind.Absolute);
            Directory.CreateDirectory(profileFolderUri.LocalPath);

            var profileUri = new Uri($"{profileFolderUri.LocalPath}/profile.json", UriKind.Absolute);
            var profileJson = JsonConvert.SerializeObject(profile, Formatting.Indented);

            Console.WriteLine($"[ProfileFileSystem] Writing profile json '{profile.Id}'");

            await WriteTextAsync(profileUri, profileJson).ConfigureAwait(false);
        }

        public async Task<IEnumerable<GameProfile>> ReadAllProfiles()
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
            // using(var stream = File.OpenText(filePath.LocalPath))
            // {
            //     return await stream.ReadToEndAsync().ConfigureAwait(false);
            // }
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