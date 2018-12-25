using Autofac;
using Avalonia;
using SpectabisNext.ComponentConfiguration;
using SpectabisNext.Interfaces;
using SpectabisNext.Views;

namespace SpectabisNext
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var container = ContainerConfiguration.Configure();            
            StartSpectabis(container);
        }

        private static void StartSpectabis(IContainer container)
        {
            using(var scope = container.BeginLifetimeScope())
            {
                var spectabisApp = scope.Resolve<ISpectabis>();
                spectabisApp.Start();
            }
        }

    }
}