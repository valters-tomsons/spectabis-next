using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileIntrinsics.Interfaces;
using FileIntrinsics.Models;

namespace FileIntrinsics
{
    public class IntrinsicsProvider : IIntrinsicsProvider
    {
        public async Task<bool> SignatureFound(string filePath, IHeaderSignature signature)
        {
            var signatureOffset = await GetSignatureOffset(filePath, signature);

            if (signatureOffset == null)
            {
                return false;
            }

            return true;
        }

        public async Task<OffsetReading> GetSignatureOffset(string filePath, IHeaderSignature signature)
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