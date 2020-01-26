using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class CueDescription : IHeaderSignature
    {
        public static IHeaderSignature Signature;

        static CueDescription()
        {
            Signature = new CueDescription();
        }

        private readonly static byte[] _signature = { 0x46, 0x49, 0x4C, 0x45, 0x20, 0x22 };
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"cue"};

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
    }
}