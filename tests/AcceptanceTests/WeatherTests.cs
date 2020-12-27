using AcceptanceTests.Core;
using AcceptanceTests.Pages;
using FluentAssertions;
using NUnit.Framework;

namespace AcceptanceTests
{
    public class WeatherTests : BaseAcceptanceTest
    {
        private WeatherPage weatherPage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            weatherPage = new WeatherPage();
            weatherPage.Navigate();
        }

        [Test]
        public void ShouldNavigateToWeather()
            => weatherPage.Header.Should().Be("The Weather");

        [Test]
        public void ShouldHaveForecastCards()
            => weatherPage.ForecastCards.Should().NotBeNullOrEmpty();
    }
}
