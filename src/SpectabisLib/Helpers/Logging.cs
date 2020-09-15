using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SpectabisLib.Helpers
{
    public static class Logging
    {
        public static void WriteLine(string message,
            [CallerFilePath] string callerPath = null)
        {
            var callerName = callerPath.Split('/').Last().Replace(".cs", string.Empty);
            Console.WriteLine($"[{callerName}] {message}");
        }
    }
}