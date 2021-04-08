using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FileIntrinsics.Interfaces;
using FileIntrinsics.Models;

namespace FileIntrinsics
{
    public class IntrinsicsProvider : IIntrinsicsProvider
    {
        private readonly IEnumerable<IHeaderSignature> _fileSignatures = new List<IHeaderSignature>();

        public IntrinsicsProvider()
        {
            _fileSignatures = EnumerateSupportedSignatures();
        }

        public async Task<IHeaderSignature> GetFileSignature(string filePath)
        {
            foreach (var sig in _fileSignatures)
            {
                var signatureFound = await SignatureFound(filePath, sig).ConfigureAwait(false);

                if(signatureFound)
                {
                    return sig;
                }
            }

            return null;
        }

        public async Task<bool> SignatureFound(string filePath, IHeaderSignature signature)
        {
            var signatureOffset = await GetSignatureOffset(filePath, signature).ConfigureAwait(false);
            return signatureOffset != null;
        }

        public async Task<OffsetReading> GetSignatureOffset(string filePath, IHeaderSignature signature)
        {
            var signatureSize = signature.ByteSignature.Length;
            var bufferSize = signature.Offsets.Max() + signatureSize;
            var fileBuffer = new byte[bufferSize];

            using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize : 4096, useAsync : true))
            {
                await stream.ReadAsync(fileBuffer, 0, bufferSize).ConfigureAwait(false);
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

        public IEnumerable<string> GetKnownExtensions()
        {
            return _fileSignatures.SelectMany(x => x.FileExtensions);
        }

        private IEnumerable<IHeaderSignature> EnumerateSupportedSignatures()
        {
            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes().Where(x => x.Namespace == "FileIntrinsics.Signatures");

            return types.Select(x => (IHeaderSignature) Activator.CreateInstance(x));
        }
    }
}