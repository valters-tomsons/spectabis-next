using System.Text;
using FileIntrinsics.Enums;
using FileIntrinsics.Interfaces;

namespace FileIntrinsics.Signatures
{
    public class FakeImage : IHeaderSignature
    {
        private readonly static byte[] _signature = Encoding.UTF8.GetBytes("!#PS2_GAME");
        private readonly static int[] _offsets = { 0x0 };
        private readonly static string[] _extensions = {"fake"};

        public FileType File => FileType.Fake;

        public byte[] ByteSignature => _signature;

        public int[] Offsets => _offsets;

        public string[] FileExtensions => _extensions;
    }
}