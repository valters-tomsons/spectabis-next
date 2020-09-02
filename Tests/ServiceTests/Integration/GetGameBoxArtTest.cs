using System;
using Bard;
using Bard.Configuration;
using ServiceClient.Interfaces;
using ServiceClient.Services;
using Xunit;

namespace ServiceTests
{
    public class GetGameBoxArtTest
    {
        private readonly IScenario Scenario;

        public GetGameBoxArtTest()
        {
            var apiUrl = Environment.GetEnvironmentVariable("ServiceBaseUrl") ?? "http://localhost:7071/api/";
            var apiKey = Environment.GetEnvironmentVariable("ServiceApiKey");

            var restClient = new RestClient();
            var baseAddress = new Uri(apiUrl, UriKind.Absolute);
            restClient.SetSession(apiKey, baseAddress);

            var client = restClient.GetClient();
            Scenario = ScenarioConfiguration.Configure(options => options.Client = client);
        }

        [Fact]
        public void When_requesting_game_art_should_return_OK()
        {
            // Act
            Scenario.When.Get("GetGameBoxArt", "serial", "SLUS21376");

            // Assert
            Scenario.Then.Response.ShouldBe.Ok();
        }
    }
}
