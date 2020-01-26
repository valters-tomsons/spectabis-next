using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileIntrinsics.Interfaces;
using FileIntrinsics.Models;
using FileIntrinsics.Signatures;

namespace FileIntrinsics
{
    public class IntrinsicsProvider : IIntrinsicsProvider
    {
        private List<IHeaderSignature> _fileSignatures = new List<IHeaderSignature>();

        public IntrinsicsProvider()
        {
            _fileSignatures.Add(new ISO9660());
            _fileSignatures.Add(new GZip());
            _fileSignatures.Add(new CueDescription());
            _fileSignatures.Add(new CISOImage());
            _fileSignatures.Add(new CD_I());
        }

        public async Task<IHeaderSignature> GetFileSignature(string filePath)
        {
            foreach (var sig in _fileSignatures)
            {
                var signatureFound = await SignatureFound(filePath, sig);

                if(signatureFound)
                {
                    return sig;
                }
            }

            return null;
        }

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