using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class ISO9660 : IHeaderSignature
    {
        public static IHeaderSignature Signature;

        static ISO9660()
        {
            Signature = new ISO9660();
        }

        private readonly static byte[] _signature = { 0x43, 0x44, 0x30, 0x30, 0x31 };
        private readonly static int[] _offsets = { 0x8001, 0x8801, 0x9001 };
        private readonly static string[] _extensions = {"iso"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
        public GameFileType File => GameFileType.ISO9660;
    }
}