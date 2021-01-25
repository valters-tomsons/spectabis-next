using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Lunr;

namespace SpectabisLib.Services
{
    public class LocalDatabaseProvider : IGameDatabaseProvider
    {
        private IEnumerable<GameMetadata> _metadataDb;
        private Lunr.Index _gameTitleIndex;
        private readonly Uri DatabaseUri = new Uri($"{SystemDirectories.ResourcesPath}/{Constants.PCSX2DatabaseName}", UriKind.Relative);

        public LocalDatabaseProvider()
        {
            _ = InitializeDatabase();
        }

        public async Task<GameMetadata> GetBySerial(string serial)
        {
            await InitializeDatabase().ConfigureAwait(false);
            return _metadataDb.FirstOrDefault(x => x.Serial == serial);
        }

        public async Task<GameMetadata> GetNearestByTitle(string title)
        {
            await InitializeDatabase().ConfigureAwait(false);

            var query = await QueryByTitle(title, 5).ConfigureAwait(false);
            return query.FirstOrDefault();
        }

        public async Task<IEnumerable<GameMetadata>> QueryByTitle(string title, int count = 5)
        {
            await InitializeDatabase().ConfigureAwait(false);

            var results = new List<GameMetadata>(count);

            await foreach (var item in _gameTitleIndex.Search(title))
            {
                var result = await GetBySerial(item.DocumentReference).ConfigureAwait(false);
                results.Add(result);
            }

            return results.Distinct(new MetadataTitleComparer()).Take(count);
        }

        public async Task InitializeDatabase()
        {
            if (_metadataDb == null)
            {
                Logging.WriteLine("Building game database");
                _metadataDb = GetDatabase().ToList();
            }

            if(_gameTitleIndex == null)
            {
                Logging.WriteLine("Building game index");
                _gameTitleIndex = await CreateIndexFromMetadata(_metadataDb).ConfigureAwait(false);
            }
        }

        private async Task<Lunr.Index> CreateIndexFromMetadata(IEnumerable<GameMetadata> data)
        {
            return await Lunr.Index.Build(async builder =>
            {
                builder.AddField("title");

                foreach(var game in data)
                {
                    await builder.Add(new Document
                    {
                        ["title"] = game.Title.Replace(" - ", String.Empty).Replace("-", String.Empty),
                        ["id"] = game.Serial
                    }).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        private IEnumerable<GameMetadata> GetDatabase()
        {
            Logging.WriteLine($"GameDatabaseProvider: Reading PCSX2 game database from '{DatabaseUri}'");

            var parserOptions = new CsvParserOptions(true, '\t');
            var csvMapper = new DatabaseModelMapping();
            var parser = new CsvParser<GameMetadata>(parserOptions, csvMapper);

            return parser.ReadFromFile(DatabaseUri.ToString(), Encoding.UTF8).Select(x => x.Result);
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

    internal class MetadataTitleComparer : IEqualityComparer<GameMetadata>
    {
        public bool Equals(GameMetadata x, GameMetadata y)
        {
            return x.Title == y.Title;
        }

        public int GetHashCode(GameMetadata obj)
        {
            var code = obj.Title;
            return code.GetHashCode();
        }
    }
}