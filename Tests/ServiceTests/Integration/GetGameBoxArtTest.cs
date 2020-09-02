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
        private readonly IRestClient Client;

        public GetGameBoxArtTest()
        {
            var baseAddress = new Uri("http://localhost:7071/api/", UriKind.Absolute);

            Client = new RestClient();
            Client.SetSession("test", baseAddress);
            var client = Client.GetClient();

            Scenario = ScenarioConfiguration.Configure(options => options.Client = client);
        }

        [Fact]
        public void When_requesting_game_art()
        {
            Scenario.When.Get("GetGameBoxArt", "serial", "SLUS21376");
            Scenario.Then.Response.ShouldBe.Ok();
        }
    }
}
