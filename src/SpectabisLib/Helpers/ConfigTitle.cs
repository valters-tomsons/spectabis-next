namespace SpectabisLib.Helpers
{
    public static class ConfigTitle
    {
        public static string ConfigClassToFileName(this string className)
        {
            return className.Replace("Config", string.Empty);
        }
    }
}