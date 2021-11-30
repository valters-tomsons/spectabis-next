using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;
using RomParsing;
using RomParsing.Parsers;
using Common.Helpers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Services
{
    public class GameFileParser : IGameFileParser
    {
        private readonly IIntrinsicsProvider _intrinsics;
        private readonly IDictionary<GameFileType, IParser> _parsers;

        public GameFileParser(IIntrinsicsProvider intrinsics, IEnumerable<IParser> parsers)
        {
            _intrinsics = intrinsics;

            // Map parsers to dictionary based on IParser.FileType
            _parsers = new Dictionary<GameFileType, IParser>(parsers.Select(x => new KeyValuePair<GameFileType, IParser>(x.FileType, x)));
        }

        public async Task<string?> GetGameSerial(string gamePath)
        {
            var fileType = await _intrinsics.GetFileSignature(gamePath).ConfigureAwait(false);

            if(fileType == null || !_parsers.ContainsKey(fileType.File))
            {
                Logging.WriteLine($"Parsing failed for unsupported file:'{gamePath}'");
                return null;
            }

            return await _parsers[fileType.File].ReadSerial(gamePath).ConfigureAwait(false);
        }
    }
}