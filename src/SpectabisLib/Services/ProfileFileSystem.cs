using System;
using System.IO;
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

            Console.WriteLine(profileJson);
            await WriteTextAsync(profileUri, profileJson).ConfigureAwait(false);
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