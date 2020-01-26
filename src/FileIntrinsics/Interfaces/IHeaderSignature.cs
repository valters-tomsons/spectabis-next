namespace FileIntrinsics.Interfaces
{
    public interface IHeaderSignature
    {
        byte[] ByteSignature { get; }
        int[] Offsets { get; }
        string[] FileExtensions { get; }
    }
}