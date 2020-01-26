using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;
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
            var fileType = await _intrinsics.GetFileSignature(gamePath);

            if (fileType.File == FileType.ISO9660)
            {
                return await ReadSerialFromIso(gamePath);
            }

            return null;
        }

        private async Task<string> ReadSerialFromIso(string gamePath)
        {
            var serialOffset = 0x828bd;
            var serialTerminator = 0x3B;
            var readLength = serialOffset + 64;
            var readBuffer = new byte[readLength];
            var indexBuffer = new byte[64];

            using(var stream = new FileStream(gamePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(readBuffer, 0, readLength);
                Array.Copy(readBuffer, serialOffset, indexBuffer, 0, 64);
            }

            var seekBuffer = new byte[64];
            int terminatorLocation = 0;
            for (int i = 0; i < indexBuffer.Length; i++)
            {
                if (indexBuffer[i] == serialTerminator)
                {
                    terminatorLocation = i;
                    break;
                }

                seekBuffer[i] = indexBuffer[i];
            }

            var resultBuffer = new byte[terminatorLocation];
            Array.Copy(seekBuffer, 0, resultBuffer, 0, terminatorLocation);

            var serialString = Encoding.UTF8.GetString(resultBuffer);
            serialString = serialString.Replace(".", string.Empty);

            return serialString;
        }
    }
}