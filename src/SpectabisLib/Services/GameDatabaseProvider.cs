using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;
using Lunr;
using System.Threading.Tasks;

namespace SpectabisLib.Services
{
    public class GameDatabaseProvider : IGameDatabaseProvider
    {
        private IEnumerable<GameMetadata> _metadataDb;
        private Index _gameTitleIndex;
        private readonly Uri DatabaseUri = new Uri($"{SystemDirectories.ResourcesPath}/gamedatabase.csv", UriKind.Relative);

        public async Task<GameMetadata> GetBySerial(string serial)
        {
            await IntializeDatabase().ConfigureAwait(false);
            return _metadataDb.FirstOrDefault(x => x.Serial == serial);
        }

        public async Task<GameMetadata> GetNearestByTitle(string title)
        {
            await IntializeDatabase().ConfigureAwait(false);

            var query = await QueryByTitle(title, 5).ConfigureAwait(false);
            return query.FirstOrDefault();
        }

        public async Task<IEnumerable<GameMetadata>> QueryByTitle(string title, int count = 5)
        {
            await IntializeDatabase().ConfigureAwait(false);

            var results = new List<GameMetadata>(count);

            await foreach (var item in _gameTitleIndex.Search(title))
            {
                var result = await GetBySerial(item.DocumentReference).ConfigureAwait(false);
                results.Add(result);
            }

            return results.Distinct(new MetadataTitleComparer()).Take(count);
        }

        public async Task IntializeDatabase()
        {
            if (_metadataDb == null)
            {
                _metadataDb = GetDatabase().ToList();
            }

            if(_gameTitleIndex == null)
            {
                _gameTitleIndex = await CreateIndexFromMetadata(_metadataDb).ConfigureAwait(false);
            }
        }

        private async Task<Index> CreateIndexFromMetadata(IEnumerable<GameMetadata> data)
        {
            return await Index.Build(async builder =>
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
            Console.WriteLine($"GameDatabaseProvider: Reading PCSX2 game database from '{DatabaseUri}'");

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