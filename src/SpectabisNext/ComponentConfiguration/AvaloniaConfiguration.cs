using Avalonia;
using Avalonia.Dialogs;
using SpectabisUI.Interfaces;

namespace SpectabisNext.ComponentConfiguration
{
    public class AvaloniaConfiguration : IWindowConfiguration
    {
        private Application Application { get; }

        public AvaloniaConfiguration()
        {
            Application = BuildAvaloniaApp().SetupWithoutStarting().Instance;
        }

        public Application GetInstance()
        {
            return Application;
        }

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect().UseManagedSystemDialogs();

    }
}