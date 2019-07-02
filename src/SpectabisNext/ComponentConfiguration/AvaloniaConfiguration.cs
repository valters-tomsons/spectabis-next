using Avalonia;
using SpectabisUI.Interfaces;

namespace SpectabisNext.ComponentConfiguration
{
    public class AvaloniaConfiguration : IWindowConfiguration
    {
        private Application application { get; set; }

        public AvaloniaConfiguration()
        {
            application = BuildAvaloniaApp().SetupWithoutStarting().Instance;
        }

        public Application GetInstance()
        {
            return application;
        }

        private static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect();

    }
}