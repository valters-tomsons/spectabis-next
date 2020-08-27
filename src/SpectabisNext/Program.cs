using Autofac;
using SpectabisNext.ComponentConfiguration;
using SpectabisUI.Interfaces;

namespace SpectabisNext
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var container = AutoFacConfiguration.Configure();
            StartSpectabis(container);
        }

        private static void StartSpectabis(IContainer container)
        {
            using var scope = container.BeginLifetimeScope();
            var spectabisApp = scope.Resolve<ISpectabis>();

            var spectabisVersion = spectabisApp.GetVersion();
            System.Console.WriteLine($"Starting Spectabis '{spectabisVersion}'");

            spectabisApp.Start();
        }
    }
}