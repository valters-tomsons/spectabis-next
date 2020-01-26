namespace FileIntrinsics.Models
{
    public class OffsetReading
    {
        public OffsetReading(int offset, byte[] reading)
        {
            Offset = offset;
            Reading = reading;
        }

        public int Offset { get; }
        public byte[] Reading {get;}
    }
}