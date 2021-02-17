using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpectabisLib.Interfaces;
using SpectabisService.Abstractions;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Providers;
using SpectabisService.Providers.Interfaces;
using SpectabisService.Services;
using SpectabisService.Services.Interfaces;

namespace SpectabisService.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSpectabisService(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClient, HttpClientFacade>();
            services.AddSingleton<IGameArtClient, GiantBombClient>();
            services.AddSingleton<IStorageProvider, AzureStorageProvider>();

            services.AddScoped<IGameDatabaseProvider, CloudDatabaseProvider>();
            services.AddScoped<IContentDownloader, ContentDownloader>();

            services.AddTransient<IGameArtProvider, GameArtProvider>();
            services.AddTransient<ContentDownloader>();

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddSingleton(_ => new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)

            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)

            .Build());

            return services;
        }
    }
}