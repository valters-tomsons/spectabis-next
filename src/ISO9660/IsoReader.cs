using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISO9660.Models;

namespace ISO9660
{
    public class IsoReader
    {
        public async Task<bool> IsIso9660(string filePath)
        {
            var signature = await FindSignature(filePath, new Signatures.ISO9660());

            if (signature == null)
            {
                return false;
            }

            return true;
        }

        public async Task<OffsetReading> FindSignature(string filePath, IHeaderSignature signature)
        {
            var signatureSize = signature.ByteSignature.Length;
            var bufferSize = signature.Offsets.Max() + signatureSize;
            var fileBuffer = new byte[bufferSize];

            using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(fileBuffer, 0, bufferSize);
            }

            for (int i = 0; i < signature.Offsets.Length; i++)
            {
                var buffer = new byte[signatureSize];
                var offset = signature.Offsets[i];
                Array.Copy(fileBuffer, offset, buffer, 0, signatureSize);

                if(buffer.SequenceEqual(signature.ByteSignature))
                {
                    return new OffsetReading(offset, buffer);
                }
            }

            return null;
        }
    }
}