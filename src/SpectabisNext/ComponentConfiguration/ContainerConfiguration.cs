using System;
using System.Linq;
using System.Reflection;
using Autofac;
using SpectabisLib.Repositories;
using SpectabisNext.Factories;
using SpectabisNext.Repositories;
using SpectabisNext.Services;
using SpectabisNext.Views;
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
            builder.RegisterNamespaceTypes(nameof(SpectabisLib.Repositories), spectabisLib);
        }

        private static void RegisterSpectabis(ContainerBuilder builder)
        {
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Repositories));
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Services));
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Views));
            builder.RegisterNamespaceTypes(nameof(SpectabisNext.Factories));

            builder.RegisterType<PageRepository>().As<IPageRepository>();
            builder.RegisterType<PageNavigator>().As<IPageNavigationProvider>();
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