using System;
using System.IO;
using System.Text;
using DiscUtils.Iso9660;

namespace FileParsing.Parsers
{
    public static class IsoParser
    {
        public static string ReadSerialFromIso(string gamePath)
        {
            var systemInfo = ReadSystemCnf(gamePath);
            return FindSerialInSystemInfo(systemInfo);
        }

        private static byte[] ReadSystemCnf(string gamePath)
        {
            using(var stream = File.Open(gamePath, FileMode.Open))
            {
                var reader = new CDReader(stream, true);
                var syscnfStream = reader.OpenFile("SYSTEM.CNF", FileMode.Open);

                if (syscnfStream == null)
                {
                    Console.WriteLine($"[IsoParser] Failed to find SYSTEM.CNF in '{gamePath}'");
                }

                var cnfBuffer = new byte[syscnfStream.Length];
                syscnfStream.Read(cnfBuffer, 0, cnfBuffer.Length);
                return cnfBuffer;
            }
        }

        private static string FindSerialInSystemInfo(byte[] cnfContent)
        {
            var cnfString = Encoding.UTF8.GetString(cnfContent);

            const char lineBreak = (char) 0x0A;
            const char carrageReturn = (char) 0x0D;

            var serialStringBuilder = new StringBuilder(cnfString.Split(lineBreak)[0]);
            serialStringBuilder.Replace(@"BOOT2 = cdrom0:\", string.Empty);
            serialStringBuilder.Replace(".", string.Empty);
            serialStringBuilder.Replace("_", string.Empty);
            serialStringBuilder.Replace(";1", string.Empty);
            serialStringBuilder.Replace($"{lineBreak}", string.Empty);
            serialStringBuilder.Replace($"{carrageReturn}", string.Empty);

            return serialStringBuilder.ToString();
        }
    }
}