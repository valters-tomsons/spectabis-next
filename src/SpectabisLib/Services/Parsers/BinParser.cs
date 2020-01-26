using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectabisLib.Enums;

namespace SpectabisLib.Services.Parsers
{
    public static class BinParser
    {
        private static int RootFileDescriptorOffset = 0xC800;
        private static int RootFileDescriptorEndOffset = 0xCFFF;
        private static byte[] DescriptionPrefix = { 0x24, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x01, 0x0D };
        private static int DescriptionTerminator = 0x3B;
        private static List<string> _gameRegions = Enum.GetNames(typeof(GameRegion)).ToList();

        public static async Task<string> ReadSerial(string gamePath)
        {
            var readBuffer = new byte[RootFileDescriptorEndOffset];

            using(var stream = new FileStream(gamePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(readBuffer, 0, RootFileDescriptorEndOffset);
            }

            var indexBufferSize = RootFileDescriptorEndOffset - RootFileDescriptorOffset;
            var indexBuffer = new byte[indexBufferSize];
            Array.Copy(readBuffer, RootFileDescriptorOffset, indexBuffer, 0, indexBufferSize);

            var serial = FindSerial(indexBuffer);
            return serial;
        }

        private static string FindSerial(byte[] fileDescriptionBuffer)
        {
            var searching = true;
            var searchIndex = 0;

            while(searching)
            {
                var descPrefixStartIndex = ByteSearch(fileDescriptionBuffer, DescriptionPrefix, searchIndex);

                if(descPrefixStartIndex == -1)
                {
                    return null;
                }

                var descBuffer = new byte[0xB];
                Array.Copy(descBuffer, descPrefixStartIndex + DescriptionPrefix.Length, descBuffer, 0, descBuffer.Length);

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

            return null;
        }

        private static string ParseDescription(byte[] buffer)
        {
            var dString = Encoding.UTF8.GetString(buffer).Replace(".", string.Empty);
            return dString;
        }

        private static int ByteSearch(byte[] searchIn, byte[] searchBytes, int start = 0)
        {
            int found = -1;
            //only look at this if we have a populated search array and search bytes with a sensible start
            if (searchIn.Length > 0 && searchBytes.Length > 0 && start <= (searchIn.Length - searchBytes.Length) && searchIn.Length >= searchBytes.Length)
            {
                //iterate through the array to be searched
                for (int i = start; i <= searchIn.Length - searchBytes.Length; i++)
                {
                    //if the start bytes match we will start comparing all other bytes
                    if (searchIn[i] == searchBytes[0])
                    {
                        if (searchIn.Length > 1)
                        {
                            //multiple bytes to be searched we have to compare byte by byte
                            bool matched = true;
                            for (int y = 1; y <= searchBytes.Length - 1; y++)
                            {
                                if (searchIn[i + y] != searchBytes[y])
                                {
                                    matched = false;
                                    break;
                                }
                            }
                            //everything matched up
                            if (matched)
                            {
                                found = i;
                                break;
                            }

                        }
                        else
                        {
                            //search byte is only one bit nothing else to do
                            found = i;
                            break; //stop the loop
                        }

                    }
                }

            }
            return found;
        }

    }
}