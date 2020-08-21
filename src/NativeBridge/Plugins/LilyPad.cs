using System.Runtime.InteropServices;

namespace NativeBridge.Plugins
{
    public class LilyPad : IPCSX2Plugin
    {
        public LilyPad()
        {

        }

        public void Configure()
        {
            NativeLilyPad64.PADconfigure();
        }
    }

    internal static class NativeLilyPad64
    {
        [DllImport("ThirdParty/libLilyPad-0.11.0.so")]
        public static extern void PADconfigure();
    }
}