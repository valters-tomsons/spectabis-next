using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SpectabisLib.Interfaces;
using SpectabisService.Abstractions;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Services;
using Microsoft.Extensions.Configuration;
using SpectabisService.Providers.Interfaces;
using SpectabisService.Providers;
using SpectabisService.Services.Interfaces;

[assembly: FunctionsStartup(typeof(SpectabisService.Startup))]
namespace SpectabisService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            services.AddConfiguration();

            services.AddSingleton<IHttpClient, HttpClientFacade>();

            services.AddTransient<IStorageProvider, AzureStorageProvider>();
            services.AddTransient<IGameDatabaseProvider, CloudDatabaseProvider>();
            services.AddTransient<IContentDownloader, ContentDownloader>();

            services.AddTransient<IGameArtClient, GiantBombClient>();
            services.AddTransient<ContentDownloader>();

            services.AddTransient<IGameArtProvider, GameArtProvider>();
        }
    }

    public static class Extensions
    {
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