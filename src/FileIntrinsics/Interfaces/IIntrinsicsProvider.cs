using System.Collections.Generic;
using System.Threading.Tasks;
using FileIntrinsics.Models;

namespace FileIntrinsics.Interfaces
{
    public interface IIntrinsicsProvider
    {
        Task<OffsetReading> GetSignatureOffset(string filePath, IHeaderSignature signature);
        Task<bool> SignatureFound(string filePath, IHeaderSignature signature);
        Task<IHeaderSignature> GetFileSignature(string filePath);
        IEnumerable<string> GetKnownExtensions();
    }
}