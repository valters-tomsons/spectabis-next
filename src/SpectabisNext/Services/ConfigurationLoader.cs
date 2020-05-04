using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Configuration;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; }
        public UIConfiguration UserInterface { get; }
        public DirectoryStruct Directories { get; }

        public ConfigurationLoader()
        {
            UserInterface = new UIConfiguration();
            Spectabis = new SpectabisConfig();
            Directories = ReadConfiguration<DirectoryStruct>().Result;
        }

        public async Task WriteConfiguration<T>(T obj) where T : IJsonConfig, new()
        {
            var configTitle = new T().Title.ToLowerInvariant();
            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            var configText = JsonConvert.SerializeObject(obj, Formatting.Indented);

            if (ConfigurationExists<T>())
            {
                File.Delete(configUri.LocalPath);
            }

            byte[] encodedText = Encoding.UTF8.GetBytes(configText);

            using var stream = File.Open(configUri.LocalPath, FileMode.OpenOrCreate);
            stream.Seek(0, SeekOrigin.End);
            await stream.WriteAsync(encodedText, 0, encodedText.Length).ConfigureAwait(false);
        }

        public bool ConfigurationExists<T>() where T : IJsonConfig, new()
        {
            var configTitle = new T().Title.ToLowerInvariant();
            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            return File.Exists(configUri.LocalPath);
        }

        public async Task<T> ReadConfiguration<T>() where T : IJsonConfig, new()
        {
            if (!ConfigurationExists<T>())
            {
                return new T();
            }

            var configTitle = new T().Title.ToLowerInvariant();
            Console.WriteLine($"[ConfigLoader] Loading '{configTitle}.json'");

            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);

            byte[] configBytes;
            using(var stream = File.Open(configUri.LocalPath, FileMode.Open))
            {
                configBytes = new byte[stream.Length];
                await stream.ReadAsync(configBytes, 0, configBytes.Length).ConfigureAwait(false);
            }

            var configText = Encoding.UTF8.GetString(configBytes);
            return JsonConvert.DeserializeObject<T>(configText);
        }
    }
}