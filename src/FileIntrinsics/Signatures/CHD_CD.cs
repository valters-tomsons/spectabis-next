using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class CHD_CD : IHeaderSignature
    {
        public static IHeaderSignature Signature;

        static CHD_CD()
        {
            Signature = new CHD_CD();
        }

        private readonly static byte[] _signature = { 0x4D, 0x43, 0x6F, 0x6D, 0x70, 0x72, 0x48, 0x44 };
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"chd"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;

        public GameFileType File => GameFileType.CHD_CD;
    }
}