using AcceptanceTests.Core;
using FluentAssertions;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public class WeatherForecastControllerTests : BaseAcceptanceTest
    {
        private const string url = "/WeatherForecast";

        private HttpResponseMessage response;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
            => response = await BackEndClient.GetAsync(url);

        [Test]
        public void ShouldReturnASuccessCode()
            => response.Should().Be2XXSuccessful();
    }
}
