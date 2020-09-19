using System.Linq;
using System.Reflection;
using Autofac;
using FileIntrinsics;
using FileIntrinsics.Interfaces;
using ServiceClient.Interfaces;
using ServiceClient.Services;
using SpectabisLib.Interfaces;
using SpectabisLib.Controllers;
using SpectabisLib.Services;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisUI.Interfaces;

namespace SpectabisNext.ComponentConfiguration
{
    public static class AutoFacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Spectabis>().As<ISpectabis>();
            builder.RegisterType<AvaloniaConfiguration>().As<IWindowConfiguration>();

            RegisterLibs(builder);
            RegisterSpectabisLib(builder);
            RegisterSpectabis(builder);
            RegisterSpectabisUI(builder);

            return builder.Build();
        }

        private static void RegisterSpectabisLib(ContainerBuilder builder)
        {
            builder.RegisterType<SpectabisLib.Repositories.GameProfileRepository>().As<IProfileRepository>().SingleInstance();
            builder.RegisterType<GameLauncherPCSX2>().As<IGameLauncher>().SingleInstance();
            builder.RegisterType<ConfigurationLoader>().As<IConfigurationLoader>().SingleInstance();
            builder.RegisterType<FirstTimeWizardController>().As<IFirstTimeWizardService>().SingleInstance();
            builder.RegisterType<LocalDatabaseProvider>().As<IGameDatabaseProvider>().SingleInstance();
            builder.RegisterType<GameArtQueue>().As<IArtServiceQueue>().SingleInstance();
            builder.RegisterType<Telemetry>().As<ITelemetry>().SingleInstance();

            builder.RegisterType<GameDirectoryScan>().As<IDirectoryScan>();
            builder.RegisterType<ProfileFileSystem>();
            builder.RegisterType<GameProfileFactory>().As<IProfileFactory>();
            builder.RegisterType<GameFileParser>().As<IGameFileParser>();
        }

        private static void RegisterSpectabis(ContainerBuilder builder)
        {
            builder.RegisterType<PageRepository>().As<IPageRepository>().SingleInstance();
            builder.RegisterType<PageNavigator>().As<IPageNavigationProvider>().SingleInstance();
            builder.RegisterType<DiscordService>().As<IDiscordService>().SingleInstance();

            builder.RegisterType<PagePreloader>().As<IPagePreloader>();
            builder.RegisterType<BitmapLoader>().As<IBitmapLoader>();
            builder.RegisterType<GifProvider>().As<IGifProvider>();
            builder.RegisterType<FileBrowser>().As<IFileBrowserFactory>();
            builder.RegisterType<ContextMenuEnumMapper>().As<IContextMenuEnumMapper>();
        }

        private static void RegisterSpectabisUI(ContainerBuilder builder)
        {
            var uiLib = Assembly.Load(nameof(SpectabisUI));

            builder.RegisterNamespaceTypes(nameof(SpectabisUI.Views), uiLib);
            builder.RegisterNamespaceTypes(nameof(SpectabisUI.Pages), uiLib);
            builder.RegisterNamespaceTypes(nameof(SpectabisUI.Factories), uiLib);
            builder.RegisterNamespaceTypes(nameof(SpectabisUI.ViewModels), uiLib);
        }

        private static void RegisterLibs(ContainerBuilder builder)
        {
            builder.RegisterType<IntrinsicsProvider>().As<IIntrinsicsProvider>();
            builder.RegisterType<RestClient>().As<IRestClient>();
            builder.RegisterType<SpectabisClient>().As<ISpectabisClient>();
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