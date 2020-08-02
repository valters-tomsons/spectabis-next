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
            await InitializeStorage().ConfigureAwait(false);
            return _metadataDb.FirstOrDefault(x => x.Serial == serial);
        }

        public async Task<GameMetadata> GetNearestByTitle(string title)
        {
            await InitializeStorage().ConfigureAwait(false);

            await foreach (var item in _gameTitleIndex.Search(title))
            {
                return await GetBySerial(item.DocumentReference).ConfigureAwait(false);
            }

            return null;
        }

        private async Task InitializeStorage()
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
            var index = await Index.Build(async builder =>
            {
                builder.AddField("title");

                foreach(var game in data)
                {
                    await builder.Add(new Document
                    {
                        ["title"] = game.Title,
                        ["id"] = game.Serial
                    }).ConfigureAwait(false);
                }
            }).ConfigureAwait(false);

            return index;
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
}