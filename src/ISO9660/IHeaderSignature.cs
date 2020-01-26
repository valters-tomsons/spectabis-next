namespace ISO9660
{
    public interface IHeaderSignature
    {
        byte[] ByteSignature { get; }
        int[] Offsets { get; }
        string[] FileExtensions { get; }
    }
}