using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpectabisLib.Configuration;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisNext.Services
{
    public class ConfigurationLoader : IConfigurationLoader
    {
        public SpectabisConfig Spectabis { get; private set; }
        public UIConfiguration UserInterface { get; private set; }
        public DirectoryStruct Directories { get; private set; }

        public ConfigurationLoader()
        {
            var update = UpdateConfiguration();
            Task.WaitAll(update);
        }

        public async Task UpdateConfiguration()
        {
            UserInterface = await ReadConfiguration<UIConfiguration>().ConfigureAwait(false);
            Spectabis = await ReadConfiguration<SpectabisConfig>().ConfigureAwait(false);
            Directories = await ReadConfiguration<DirectoryStruct>().ConfigureAwait(false);
        }

        public bool ConfigurationExists<T>() where T : IJsonConfig, new()
        {
            var configTitle = new T().Title.ToLowerInvariant();
            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            return File.Exists(configUri.LocalPath);
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

            Logging.WriteLine($"Writing to file '{configUri.LocalPath}'");
            await AsyncIOHelper.WriteTextToFile(configUri, configText).ConfigureAwait(false);
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
                return ReturnDefault<T>();
            }

            var configTitle = new T().Title.ToLowerInvariant();
            Logging.WriteLine($"Loading '{configTitle}.json'");

            var configUri = new Uri($"{SystemDirectories.ConfigFolder}/{configTitle}.json", UriKind.Absolute);
            var configText = await AsyncIOHelper.ReadTextFromFile(configUri).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(configText);
        }

        private T ReturnDefault<T>() where T : IJsonConfig, new()
        {
            Logging.WriteLine($"Getting default config for '{typeof(T)}'");
            return new T();
        }
    }
}