using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace RobotFactory.WebApi.Tests
{
    public class WebAppTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;

        public WebAppTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task Api_IsResponding()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/health-check");

            Assert.True(response.IsSuccessStatusCode);
        }
    }
}