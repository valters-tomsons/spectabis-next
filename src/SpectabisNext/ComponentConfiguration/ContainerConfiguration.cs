using System.Linq;
using System.Reflection;
using Autofac;
using SpectabisLib.Interfaces;
using SpectabisLib.Services;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisUI.Interfaces;

namespace SpectabisNext.ComponentConfiguration
{
    public static class ContainerConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Spectabis>().As<ISpectabis>();
            builder.RegisterType<AvaloniaConfiguration>().As<IWindowConfiguration>();

            RegisterSpectabisLib(builder);
            RegisterSpectabis(builder);

            return builder.Build();
        }

        private static void RegisterSpectabisLib(ContainerBuilder builder)
        {
            var spectabisLib = Assembly.Load(nameof(SpectabisLib));
            builder.RegisterType<SpectabisLib.Repositories.GameProfileRepository>().As<IProfileRepository>().SingleInstance();
            builder.RegisterType<SpectabisLib.Repositories.CancellationTokenRepository>().SingleInstance();
            builder.RegisterType<GameLauncherPCSX2>().As<IGameLauncher>().SingleInstance();
        }

        private static void RegisterSpectabis(ContainerBuilder builder)
        {
            builder.RegisterType<PageRepository>().As<IPageRepository>().SingleInstance();
            builder.RegisterType<PageNavigator>().As<IPageNavigationProvider>().SingleInstance();
            builder.RegisterType<ConfigurationLoader>().As<IConfigurationLoader>().SingleInstance();
           
            builder.RegisterType<PagePreloader>().As<IPagePreloader>();
            builder.RegisterType<BitmapLoader>().As<IBitmapLoader>();
            builder.RegisterType<GameProfileFactory>().As<IProfileFactory>();

            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Views));
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Pages));
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Factories));
        }

        private static ContainerBuilder RegisterNamespaceTypes(this ContainerBuilder builder, string targetNamespace, Assembly assembly = null)
        {
            if(assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }

            var namespaceTypes = assembly.GetTypes().Where(x => x.Namespace.Contains(targetNamespace)).ToArray();
            builder.RegisterTypes(namespaceTypes);
            return builder;
        }
    }
}