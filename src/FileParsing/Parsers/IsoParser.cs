using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileParsing.Parsers
{
    public static class IsoParser
    {
        public static async Task<string> ReadSerialFromIso(string gamePath)
        {
            const int serialOffset = 0x828bd;
            const int serialTerminator = 0x3B;
            const int readLength = serialOffset + 64;
            var readBuffer = new byte[readLength];
            var indexBuffer = new byte[64];

            using(var stream = new FileStream(gamePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(readBuffer, 0, readLength).ConfigureAwait(false);
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
            return serialString.Replace(".", string.Empty);
        }
    }
}