using System.Text;

namespace SpectabisLib.Helpers
{
    public static class GameSerialNormalizer
    {
        public static string NormalizeSerial(this string serial)
        {
            var sb = new StringBuilder(serial);
            sb.Replace("=", string.Empty);
            sb.Replace("-", string.Empty);
            sb.Replace(" ", string.Empty);
            return sb.ToString();
        }
    }
}