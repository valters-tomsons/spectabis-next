using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;
using SpectabisLib.Interfaces;
using SpectabisLib.Services.Parsers;

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

            if (fileType.File == FileType.ISO9660)
            {
                return await IsoParser.ReadSerialFromIso(gamePath).ConfigureAwait(false);
            }

            if(fileType.File == FileType.CD_I)
            {
                return await BinParser.ReadSerial(gamePath).ConfigureAwait(false);
            }

            return null;
        }

    }
}