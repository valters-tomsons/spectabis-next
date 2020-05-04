using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SpectabisLib.Helpers
{
    public static class AsyncIOHelper
    {
        public async static Task WriteTextToFile(Uri destination, string text, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var textBytes = encoding.GetBytes(text);

            using(var stream = new FileStream(destination.LocalPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
            {
                await stream.WriteAsync(textBytes, 0, textBytes.Length).ConfigureAwait(false);
            }
        }

        public async static Task<string> ReadTextFromFile(Uri source, Encoding encoding = null)
        {
            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] readBuffer;

            using(var stream = new FileStream(source.LocalPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            {
                readBuffer = new byte[stream.Length];
                await stream.ReadAsync(readBuffer, 0, readBuffer.Length).ConfigureAwait(false);
            }

            return encoding.GetString(readBuffer);
        }
    }
}