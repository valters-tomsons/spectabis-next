namespace ISO9660.Models
{
    public class HeaderInformation
    {
        public HeaderInformation(byte[] bytePattern, int? patternOffset, bool patternFound)
        {
            BytePattern = bytePattern;
            PatternFound = patternFound;

            if (patternFound)
            {
                PatternOffset = patternOffset;
            }
            else
            {
                PatternOffset = null;
            }
        }

        public byte[] BytePattern { get; }
        public int? PatternOffset { get; }
        public bool PatternFound { get; }
    }
}