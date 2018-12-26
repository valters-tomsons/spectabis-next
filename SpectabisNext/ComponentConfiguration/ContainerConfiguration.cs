using Autofac;
using SpectabisLib.Repositories;
using SpectabisNext.Factories;
using SpectabisNext.Interfaces;
using SpectabisNext.Models.Configuration;
using SpectabisNext.Views;

namespace SpectabisNext.ComponentConfiguration
{
    public class ContainerConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Spectabis>().As<ISpectabis>();

            // Contains avalonia application instance
            builder.RegisterType<AvaloniaConfiguration>().As<IWindowConfiguration>();
            builder.RegisterType<MainWindow>();

            builder.RegisterType<GameLibrary>();
            builder.RegisterType<Settings>();

            builder.RegisterType<UIConfiguration>();

            builder.RegisterType<GameProfileRepository>();

            builder.RegisterType<GameTileFactory>();

            return builder.Build();
        }
    }
}