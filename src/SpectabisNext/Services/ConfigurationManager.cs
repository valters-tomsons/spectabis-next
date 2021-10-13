using System.Text;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Common.Helpers;
using SpectabisLib.Configuration;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationManager : IConfigurationManager
    {
        public SpectabisConfig Spectabis { get; set; }
        public UIConfig UserInterface { get; set; }
        public DirectoryConfig Directories { get; set; }
        public TextConfig TextConfig { get; set; }

        private readonly Encoding Encoding = Encoding.UTF8;

        public ConfigurationManager()
        {
            var update = UpdateConfiguration();
            Task.WaitAll(update);
        }

        public async Task UpdateConfiguration()
        {
            UserInterface = await ReadConfiguration<UIConfig>().ConfigureAwait(false);
            Spectabis = await ReadConfiguration<SpectabisConfig>().ConfigureAwait(false);
            Directories = await ReadConfiguration<DirectoryConfig>().ConfigureAwait(false);
            TextConfig = await ReadConfiguration<TextConfig>().ConfigureAwait(false);
        }

        public bool ConfigurationExists<T>() where T : IJsonConfig, new()
        {
            var configUri = GetConfigUri<T>();
            return File.Exists(configUri.LocalPath);
        }

        public async Task WriteConfiguration<T>(T obj) where T : IJsonConfig, new()
        {
            var configUri = GetConfigUri<T>();
            var configText = JsonConvert.SerializeObject(obj, Formatting.Indented);

            if (ConfigurationExists<T>())
            {
                File.Delete(configUri.LocalPath);
            }

            await File.WriteAllTextAsync(configUri.LocalPath, configText, Encoding).ConfigureAwait(false);
        }

        public async Task WriteDefaultsIfNotExist<T>() where T : IJsonConfig, new()
        {
            if(ConfigurationExists<T>())
            {
                return;
            }

            var obj = new T();
            await WriteConfiguration<T>(obj).ConfigureAwait(false);
        }

        public async Task<T> ReadConfiguration<T>() where T : IJsonConfig, new()
        {
            if (!ConfigurationExists<T>())
            {
                Logging.WriteLine($"Getting default config for '{typeof(T)}'");
                return new T();
            }

            var configUri = GetConfigUri<T>();
            Logging.WriteLine($"Loading '{configUri.LocalPath}");

            var configText = await File.ReadAllTextAsync(configUri.LocalPath, Encoding).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(configText);
        }

        private static Uri GetConfigUri<T>() where T : IJsonConfig, new()
        {
            var configFolder = SystemDirectories.ConfigFolder;
            var title = new T().ConfigName.ToLowerInvariant();

            return new Uri($"{configFolder}/{title}.json", UriKind.Absolute);
        }
    }
}