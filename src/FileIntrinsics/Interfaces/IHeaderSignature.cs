using FileIntrinsics.Enums;

namespace FileIntrinsics.Interfaces
{
    public interface IHeaderSignature
    {
        FileType File { get; }
        byte[] ByteSignature { get; }
        int[] Offsets { get; }
        string[] FileExtensions { get; }
    }
}