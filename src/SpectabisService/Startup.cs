using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using GiantBomb.Api;
using SpectabisLib.Interfaces;
using SpectabisService.Abstractions;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Services;

[assembly: FunctionsStartup(typeof(SpectabisService.Startup))]
namespace SpectabisService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var giantBombApiKey = Environment.GetEnvironmentVariable("ApiKey_GiantBomb");
            var storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

            var services = builder.Services;

            services.AddSingleton<IHttpClient, HttpClientFacade>();

            services.AddSingleton<IGiantBombRestClient>(_ => new GiantBombRestClient(giantBombApiKey));
            services.AddSingleton<IStorageProvider>(_ => new StorageProvider(storageConnectionString));
            services.AddSingleton<PCSX2DatabaseProvider>();

            services.AddScoped<IGameArtClient, GiantBombClient>();
            services.AddScoped<ContentDownloader>();
        }
    }
}