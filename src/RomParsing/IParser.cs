using System.Threading.Tasks;
using FileIntrinsics.Enums;

namespace RomParsing
{
    public interface IParser
    {
        Task<string?> ReadSerial(string filePath);
        GameFileType FileType { get; }
    }
}