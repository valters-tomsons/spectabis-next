using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpectabisService.Extensions;
using SpectabisService.Services;

[assembly: FunctionsStartup(typeof(SpectabisService.Startup))]
namespace SpectabisService
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddApplicationInsightsTelemetry();

            services.AddConfiguration();
            services.AddSpectabisService();
        }
    }
}