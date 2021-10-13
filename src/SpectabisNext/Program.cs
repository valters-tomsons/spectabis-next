using Autofac;
using SpectabisNext.ComponentConfiguration;
using SpectabisUI.Interfaces;
using Common.Helpers;
using SpectabisLib.Interfaces.Services;
using System.Threading.Tasks;

namespace SpectabisNext
{
    internal static class Program
    {
        private async static Task Main(string[] args)
        {
            var container = AutoFacConfiguration.Configure();
            await StartSpectabis(container, args).ConfigureAwait(false);

            var telemetry = container.Resolve<ServiceLib.Interfaces.ITelemetry>();
            telemetry.Flush();
        }

        private async static Task StartSpectabis(IContainer container, string[] arguments)
        {
            using var scope = container.BeginLifetimeScope();

            if (arguments?.Length > 0)
            {
                var cli = scope.Resolve<ICommandLineService>();
                var skipApp = await cli.ExecuteArguments(arguments).ConfigureAwait(false);

                if(skipApp) return;
            }

            var spectabisApp = scope.Resolve<ISpectabis>();

            var spectabisVersion = spectabisApp.GetVersion();
            Logging.WriteLine($"Starting Spectabis '{spectabisVersion}'");

            spectabisApp.Start();
        }
    }
}