using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SpectabisLib.Enums;
using SpectabisLib.Helpers;
using SpectabisLib.Interfaces;
using SpectabisLib.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace SpectabisLib.Services
{
    public class GameDatabaseProvider : IGameDatabaseProvider
    {
        private IEnumerable<GameMetadata> _metadataDb;
        private readonly Uri DatabaseUri = new Uri($"{SystemDirectories.ResourcesPath}/gamedatabase.csv", UriKind.Relative);

        public GameMetadata GetBySerial(string serial)
        {
            if (_metadataDb == null)
            {
                _metadataDb = GetDatabase().ToList();
            }

            return _metadataDb.FirstOrDefault(x => x.Serial == serial);
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