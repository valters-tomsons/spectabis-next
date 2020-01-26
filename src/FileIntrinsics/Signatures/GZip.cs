using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class GZip : IHeaderSignature
    {
        
        public static IHeaderSignature Signature;

        static GZip()
        {
            Signature = new GZip();
        }

        private readonly static byte[] _signature = {0x1F, 0x8B, 0x08};
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"iso.gz"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
        public FileType File => FileType.GZip;
    }
}