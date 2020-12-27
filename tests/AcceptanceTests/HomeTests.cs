using AcceptanceTests.Core;
using AcceptanceTests.Pages;
using FluentAssertions;
using NUnit.Framework;

namespace AcceptanceTests
{
    public class HomeTests : BaseAcceptanceTest
    {
        private HomePage homePage;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            homePage = new HomePage();
            homePage.Navigate();
        }

        [Test]
        public void ShouldNavigateToHome()
        {
            homePage.Header.Should().Be("Home Page");
        }
    }
}
