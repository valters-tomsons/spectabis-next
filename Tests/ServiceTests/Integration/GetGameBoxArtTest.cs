using System;
using System.Net.Http;
using Bard;
using Bard.Configuration;
using Xunit;

namespace ServiceTests
{
    public class GetGameBoxArtTest
    {
        private readonly IScenario Scenario;

        public GetGameBoxArtTest()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:7071/api/")
            };

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
