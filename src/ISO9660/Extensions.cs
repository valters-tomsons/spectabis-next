namespace ISO9660
{
    public static class Extensions
    {
        public static bool Contains(this byte[] buffer, ref byte[] sequence, out int position)
        {
            int currOffset = 0;

            for (position = 0; position < buffer.Length; position++)
            {
                byte b = buffer[position];
                if (b == sequence[currOffset])
                {
                    if (currOffset == sequence.Length - 1) return true;
                    currOffset++;
                    continue;
                }

                // Fixup the offset to the byte after the beginning of the abortive sequence
                if (currOffset == 0) continue;
                position -= currOffset;
                currOffset = 0;
            }

            return false;
        }
    }
}