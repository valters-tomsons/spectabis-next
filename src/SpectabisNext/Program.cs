using Autofac;
using SpectabisNext.ComponentConfiguration;
using SpectabisUI.Interfaces;
using SpectabisLib.Helpers;

namespace SpectabisNext
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var container = AutoFacConfiguration.Configure();
            StartSpectabis(container);

            var telemetry = container.Resolve<ServiceLib.Interfaces.ITelemetry>();
            telemetry.Flush();
        }

        private static void StartSpectabis(IContainer container)
        {
            using var scope = container.BeginLifetimeScope();
            var spectabisApp = scope.Resolve<ISpectabis>();

            var spectabisVersion = spectabisApp.GetVersion();
            Logging.WriteLine($"Starting Spectabis '{spectabisVersion}'");

            spectabisApp.Start();
        }
    }
}