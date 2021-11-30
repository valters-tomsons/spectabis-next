using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SpectabisLib.Enums;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace SpectabisService.Services
{
    public class CloudDatabaseProvider : IGameDatabaseProvider
    {
        private readonly IHttpClient _httpClient;
        private readonly IStorageProvider _storageProvider;
        private readonly Uri _databaseUri;

        private const string DatabaseFileName = "PCSX2_GAMES";
        private readonly TimeSpan DatabaseRefreshInterval = TimeSpan.FromDays(5);

        public CloudDatabaseProvider(IHttpClient httpClient, IConfigurationRoot config, IStorageProvider storageProvider)
        {
            var databaseUrl = config.GetValue<string>("DatabaseUri_PCSX2");
            _databaseUri = new Uri(databaseUrl ,UriKind.Absolute);

            _httpClient = httpClient;
            _storageProvider = storageProvider;
        }

        public async Task<GameMetadata?> GetBySerial(string serial)
        {
            var db = await GetDatabase().ConfigureAwait(false);
            return db.FirstOrDefault(x => x.Serial.Equals(serial));
        }

        #pragma warning disable CS1998
        public async Task<GameMetadata?> GetNearestByTitle(string title)
        {
            throw new NotSupportedException();
        }

        public async Task<IEnumerable<GameMetadata>> QueryByTitle(string title, int count)
        {
            throw new NotSupportedException();
        }

        public async Task InitializeDatabase()
        {
            throw new NotSupportedException();
        }
        #pragma warning restore CS1998

        private async Task<IEnumerable<GameMetadata>> GetDatabase()
        {
            var fromStorage = ReadParsedFromStorage().ConfigureAwait(false);
            var lastModified = await _storageProvider.GetLastModified(DatabaseFileName).ConfigureAwait(false);

            if(!lastModified.HasValue || lastModified == null)
            {
                await UpdateStorageDatabase().ConfigureAwait(false);
            }

            if(!lastModified.HasValue)
            {
                lastModified = DateTimeOffset.Now.Subtract(DatabaseRefreshInterval);
            }

            var redownload = DateTimeOffset.Now - lastModified.Value > DatabaseRefreshInterval;

            if (redownload || await fromStorage == null)
            {
                await UpdateStorageDatabase().ConfigureAwait(false);
            }

            return await fromStorage;
        }

        private async Task<IEnumerable<GameMetadata>> UpdateStorageDatabase()
        {
            var fromSource = await GetDatabaseFromSource().ConfigureAwait(false);
            await UploadDatabaseToStorage(fromSource).ConfigureAwait(false);
            return fromSource;
        }

        private async Task<IEnumerable<GameMetadata>> GetDatabaseFromSource()
        {
            var dbContent = await _httpClient.GetAsync(_databaseUri).ConfigureAwait(false);
            var contentStream = await dbContent.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var parserOptions = new CsvParserOptions(true, '\t');
            var csvMapper = new DatabaseModelMapping();
            var parser = new CsvParser<GameMetadata>(parserOptions, csvMapper);

            return parser.ReadFromStream(contentStream, Encoding.UTF8).Select(x => x.Result).ToList();
        }

        private async Task UploadDatabaseToStorage(IEnumerable<GameMetadata> data)
        {
            var memoryStream = new MemoryStream();

            using(var writer = new BsonDataWriter(memoryStream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, new MetadataStorageFile(data));
            }

            await _storageProvider.WriteDataToStorage(DatabaseFileName, memoryStream.ToArray()).ConfigureAwait(false);
        }

        private async Task<IEnumerable<GameMetadata>> ReadParsedFromStorage()
        {
            var buffer = await _storageProvider.ReadBytesFromStorage(DatabaseFileName).ConfigureAwait(false);

            if(buffer == null)
            {
                return Enumerable.Empty<GameMetadata>();
            }

            var memoryStream = new MemoryStream(buffer);
            using var reader = new BsonDataReader(memoryStream);
            var serializer = new JsonSerializer();

            var deserialized = serializer.Deserialize<MetadataStorageFile>(reader);

            if(deserialized is null)
            {
                return Enumerable.Empty<GameMetadata>();
            }

            return deserialized.Metadata;
        }
    }

    internal class DatabaseModelMapping : CsvMapping<GameMetadata>
    {
        public DatabaseModelMapping()
        {
            MapProperty(0, x => x.Serial);
            MapProperty(1, x => x.Compatibility, new EnumConverter<GameCompatibility>(true));
            MapProperty(5, x => x.Title);
            MapProperty(6, x => x.Region);
        }
    }
}