using AcceptanceTests.Core;
using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    public class FrontEndSmokeTests
    {
        private bool hasServer;
        private NpmScript frontEndServer;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            frontEndServer = new NpmScript("start");
            await frontEndServer.RunAsync(x => Debug.WriteLine(x));

            hasServer = frontEndServer.HasServer;
        }

        [Test]
        public void ShouldHaveAServer()
            => hasServer.Should().BeTrue();

        [OneTimeTearDown]
        public void OneTimeTearDown()
            => frontEndServer.Dispose();
    }
}