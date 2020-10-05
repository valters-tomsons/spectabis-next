using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RomParsing.Enums;
using RomParsing.Utils;

namespace RomParsing.Parsers
{
    public static class BinParser
    {
        private const int RootFileDescriptorOffset = 0xC800;
        private const int RootFileDescriptorEndOffset = 0xCFFF;
        private static readonly byte[] DescriptionPrefix = { 0x24, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x0D };
        private static readonly List<string> _gameRegions = Enum.GetNames(typeof(GameRegion)).ToList();

        public static async Task<string> ReadSerial(string gamePath)
        {
            var readBuffer = new byte[RootFileDescriptorEndOffset];

            using(var stream = new FileStream(gamePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(readBuffer, 0, RootFileDescriptorEndOffset).ConfigureAwait(false);
            }

            const int indexBufferSize = RootFileDescriptorEndOffset - RootFileDescriptorOffset;

            var indexBuffer = new byte[indexBufferSize];
            Array.Copy(readBuffer, RootFileDescriptorOffset, indexBuffer, 0, indexBufferSize);

            var serial = FindSerial(indexBuffer);
            return serial.Replace("_", string.Empty);
        }

        private static string FindSerial(byte[] fileDescriptionBuffer)
        {
            const bool searching = true;
            var searchIndex = 0;

            while(searching)
            {
                var descPrefixStartIndex = ByteSearch.FindPattern(fileDescriptionBuffer, DescriptionPrefix, searchIndex);

                if(descPrefixStartIndex == -1)
                {
                    return null;
                }

                var descBuffer = new byte[0xB];
                Array.Copy(fileDescriptionBuffer, descPrefixStartIndex + DescriptionPrefix.Length, descBuffer, 0, descBuffer.Length);

                var descString = ParseDescription(descBuffer);

                foreach (var region in _gameRegions)
                {
                    if(descString.Contains(region))
                    {
                        return descString;
                    }
                }

                searchIndex = descPrefixStartIndex + descBuffer.Length;
            }
        }

        private static string ParseDescription(byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer).Replace(".", string.Empty);
        }
    }
}