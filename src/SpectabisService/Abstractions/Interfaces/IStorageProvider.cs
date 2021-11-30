using System;
using System.Threading.Tasks;

namespace SpectabisService.Abstractions.Interfaces
{
    public interface IStorageProvider
    {
        Task<DateTimeOffset?> GetLastModified(string fileName);
        Task InitializeStorage();
        Task<byte[]?> ReadBytesFromStorage(string fileName);
        Task WriteDataToStorage(string fileName, byte[] buffer);
        Task WriteImageToStorage(string fileName, byte[] buffer);
    }
}