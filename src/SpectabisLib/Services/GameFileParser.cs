using System;
using System.Threading.Tasks;
using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;
using FileParsing.Parsers;
using SpectabisLib.Interfaces;

namespace SpectabisLib.Services
{
    public class GameFileParser : IGameFileParser
    {
        private readonly IIntrinsicsProvider _intrinsics;

        public GameFileParser(IIntrinsicsProvider intrinsics)
        {
            _intrinsics = intrinsics;
        }

        public async Task<string> GetGameSerial(string gamePath)
        {
            var fileType = await _intrinsics.GetFileSignature(gamePath).ConfigureAwait(false);

            if(fileType == null)
            {
                Console.WriteLine($"[GameFileParser] parsing failed for unsupported file:'{gamePath}'");
                return null;
            }

            if (fileType.File == FileType.ISO9660)
            {
                return IsoParser.ReadSerialFromIso(gamePath);
            }

            if(fileType.File == FileType.CD_I)
            {
                return await BinParser.ReadSerial(gamePath).ConfigureAwait(false);
            }

            return null;
        }
    }
}