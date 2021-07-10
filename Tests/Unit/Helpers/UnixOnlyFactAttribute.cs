using System.Runtime.InteropServices;
using Xunit;

namespace Unit.Helpers
{
    public sealed class UnixOnlyFact : FactAttribute
    {
        public UnixOnlyFact()
        {
            if (!(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX)))
            {
                Skip = "Test requires a POSIX compliant shell at '/bin/sh'";
            }
        }
    }
}