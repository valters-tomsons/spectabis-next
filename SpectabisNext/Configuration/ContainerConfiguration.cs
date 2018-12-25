using Autofac;
using SpectabisLib.Services;
using SpectabisNext.Interfaces;
using SpectabisNext.Views;

namespace SpectabisNext.Configuration
{
    public class ContainerConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Spectabis>().As<ISpectabis>();
            builder.RegisterType<AvaloniaConfiguration>().As<IWindowConfiguration>();
            builder.RegisterType<MainWindow>();

            builder.RegisterType<GameProfileRepository>();

            return builder.Build();
        }
    }
}