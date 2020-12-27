using AcceptanceTests.Core;
using FluentAssertions;
using NUnit.Framework;

namespace AcceptanceTests
{
    public class FrontEndSmokeTests : BaseAcceptanceTest
    {
        [Test]
        public void ShouldHaveAUrl()
            => FrontEndServer.HasUrl.Should().BeTrue();
    }
}