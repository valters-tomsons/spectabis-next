using System.Linq;
using System.Reflection;
using Autofac;
using FileIntrinsics;
using FileIntrinsics.Interfaces;
using ServiceLib.Interfaces;
using ServiceLib.Services;
using SpectabisUI.Controllers;
using SpectabisLib.Interfaces;
using SpectabisLib.Services;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisUI.Interfaces;
using SpectabisLib.Abstractions;
using SpectabisLib.Interfaces.Controllers;
using SpectabisLib.Interfaces.Services;
using EmuConfig.Interfaces;
using EmuConfig;
using RomParsing.Parsers;
using RomParsing;
using SpectabisLib.Interfaces.Abstractions;
using System.Runtime.InteropServices;

namespace SpectabisNext.ComponentConfiguration
{
    public static class AutoFacConfiguration
    {
        [ComVisible(false)]
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Spectabis>().As<ISpectabis>();
            builder.RegisterType<AvaloniaConfiguration>().As<IWindowConfiguration>();

            RegisterLibs(builder);
            RegisterSpectabisLib(builder);
            RegisterRomParsers(builder);

            RegisterSpectabis(builder);
            RegisterSpectabisUI(builder);

            builder.RegisterType<IniParser>().As<IParserProvider>();

            return builder.Build();
        }

        private static void RegisterSpectabisLib(ContainerBuilder builder)
        {
            builder.RegisterType<SpectabisLib.Repositories.GameProfileRepository>().As<IProfileRepository>().SingleInstance();
            builder.RegisterType<GameLauncherPCSX2>().As<IGameLauncher>().SingleInstance();
            builder.RegisterType<ConfigurationManager>().As<IConfigurationManager>().SingleInstance();
            builder.RegisterType<LocalDatabaseProvider>().As<IGameDatabaseProvider>().SingleInstance();
            builder.RegisterType<GameArtQueue>().As<IArtServiceQueue>().SingleInstance();
            builder.RegisterType<Telemetry>().As<ITelemetry>().SingleInstance();

            builder.RegisterType<FirstTimeWizardController>().As<IFirstTimeWizardService>().SingleInstance();
            builder.RegisterType<SettingsController>().As<ISettingsController>().SingleInstance();
            builder.RegisterType<GameLibraryController>().As<IGameLibraryController>().SingleInstance();
            builder.RegisterType<GameFileParser>().As<IGameFileParser>().SingleInstance();

            builder.RegisterType<GameDirectoryScan>().As<IDirectoryScan>();
            builder.RegisterType<ProfileFileSystem>().As<IProfileFileSystem>();
            builder.RegisterType<GameProfileFactory>().As<IProfileFactory>();
            builder.RegisterType<GameConfigurationService>().As<IGameConfigurationService>();
            builder.RegisterType<LocalCachingService>().As<ILocalCachingService>();
            builder.RegisterType<CommandLineService>().As<ICommandLineService>();
        }

        private static void RegisterSpectabis(ContainerBuilder builder)
        {
            builder.RegisterType<PageRepository>().As<IPageRepository>().SingleInstance();
            builder.RegisterType<PageNavigator>().As<IPageNavigationProvider>().SingleInstance();
            builder.RegisterType<DiscordService>().As<IDiscordService>().SingleInstance();
            builder.RegisterType<FileBrowser>().As<IFileBrowserFactory>().SingleInstance();

            builder.RegisterType<PagePreloader>().As<IPagePreloader>();
            builder.RegisterType<BitmapLoader>().As<IBitmapLoader>();
            builder.RegisterType<GifProvider>().As<IGifProvider>();
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

        private static void RegisterRomParsers(ContainerBuilder builder)
        {
            builder.RegisterType<IsoParser>().As<IParser>();
            builder.RegisterType<BinParser>().As<IParser>();
            builder.RegisterType<FakeParser>().As<IParser>();
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