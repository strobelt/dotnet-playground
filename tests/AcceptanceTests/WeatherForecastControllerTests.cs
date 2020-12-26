using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi;

namespace AcceptanceTests
{
    public class WeatherForecastControllerTests
    {
        private const string Url = "/WeatherForecast";

        private HttpClient _client;
        private HttpResponseMessage _response;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());

            _client = server.CreateClient();
            _response = await _client.GetAsync(Url);
        }

        [Test]
        public void ShouldReturnASuccessCode()
            => _response.Should().Be2XXSuccessful();
    }
}