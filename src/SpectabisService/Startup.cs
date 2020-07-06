using SpectabisLib.Interfaces;
using SpectabisService.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using SpectabisService.Abstractions.Interfaces;
using SpectabisService.Abstractions;

[assembly: FunctionsStartup(typeof(SpectabisService.Startup))]
namespace SpectabisService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<IHttpClient, HttpClientFacade>();

            services.AddScoped<IGameArtClient, GiantBombClient>();

            services.AddScoped<PCSX2DatabaseProvider>();
            services.AddScoped<ArtCacheProvider>();
            services.AddScoped<ContentDownloader>();
        }
    }
}