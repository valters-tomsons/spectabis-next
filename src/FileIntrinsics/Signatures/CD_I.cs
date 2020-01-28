using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class CD_I : IHeaderSignature
    {
        public static IHeaderSignature Signature;

        static CD_I()
        {
            Signature = new CD_I();
        }

        private readonly static byte[] _signature = {0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x02, 0x00, 0x02};
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"bin"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;

        public FileType File => FileType.CD_I;
    }
}