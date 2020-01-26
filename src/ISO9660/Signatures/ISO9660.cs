using ISO9660.Models;

namespace ISO9660.Signatures
{
    public class ISO9660 : IHeaderSignature
    {
        private readonly static int[] _offsets = { 0x8001, 0x8801, 0x9001 };
        private readonly static string[] _extensions = {".iso"};

        public byte[] ByteSignature => Constants.ISO9660;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
    }
}