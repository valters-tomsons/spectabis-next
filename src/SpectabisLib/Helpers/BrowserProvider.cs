using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SpectabisLib.Helpers
{
    public static class BrowserProvider
    {
        public static void OpenWebBrowser(Uri browserUrl)
        {
            var url = new StringBuilder(browserUrl.AbsoluteUri);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {browserUrl}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url.ToString());
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url.ToString());
            }
            else{
                throw new PlatformNotSupportedException();
            }
        }
    }
}