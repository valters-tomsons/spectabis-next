using System;
using Bard;
using Bard.Configuration;
using ServiceLib.Services;
using Xunit;

namespace Tests.Integration.ServiceTests
{
    public class GetGameBoxArtTest
    {
        private readonly IScenario Scenario;

        private readonly string endpoint = ServiceLib.Constants.Endpoints.GetBySerial;
        private readonly string serialQuery = "serial";
        private readonly string validSerial = "SLUS21376";

        public GetGameBoxArtTest()
        {
            var apiUrl = Environment.GetEnvironmentVariable("SERVICE_BASE_URL") ?? "http://localhost:7071/api/";
            var apiKey = Environment.GetEnvironmentVariable("SERVICE_API_KEY");

            var restClient = new RestClient();
            var baseAddress = new Uri(apiUrl, UriKind.Absolute);
            restClient.SetSession(apiKey, baseAddress);

            var client = restClient.GetClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            Scenario = ScenarioConfiguration.Configure(options => options.Client = client);
        }

        [Fact]
        public void GivenValidSerial_GetBySerial_ReturnsOk()
        {
            // Act
            Scenario.When.Get(endpoint, serialQuery, validSerial);

            // Assert
            Scenario.Then.Response.ShouldBe.Ok();
        }

        [Fact]
        public void GivenValidSerial_GetBySerial_ResultContentTypeAsExpected()
        {
            // Act
            Scenario.When.Get(endpoint, serialQuery, validSerial);

            // Assert
            Scenario.Then.Response.Headers.Should.Include.ContentType("image/png");
        }

        [Fact]
        public void GivenKnownImageSize_GetBySerial_ResultContentLengthAsExpected()
        {
            // Act
            Scenario.When.Get(endpoint, serialQuery, validSerial);

            // Assert
            Scenario.Then.Response.Headers.Should.Include.ContentLength("739028");
        }
    }
}
