using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Common.Helpers
{
    public static class Logging
    {
        public static void WriteLine(string message,
            [CallerFilePath] string? callerPath = null)
        {
            var callerName = callerPath?.Split(Path.DirectorySeparatorChar).Last();
            Console.WriteLine($"[{callerName}] {message}");
        }
    }
}