using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class CISOImage : IHeaderSignature
    {
        public static IHeaderSignature Signature;

        static CISOImage()
        {
            Signature = new CISOImage();
        }

        private readonly static byte[] _signature = {0x43, 0x49, 0x53, 0x4F};
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"cso"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
        public FileType File => FileType.CISOImage;
    }
}