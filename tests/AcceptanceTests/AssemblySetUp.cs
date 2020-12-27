using AcceptanceTests.Core;
using NUnit.Framework;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [SetUpFixture]
    public class AssemblySetUp
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            BaseAcceptanceTest.FrontEndServer = new NpmScript("start");
            await BaseAcceptanceTest.FrontEndServer.RunAsync(x => Debug.WriteLine(x));
            Page.BaseUrl = BaseAcceptanceTest.FrontEndServer.Url;

            SeleniumDriverManager.Start();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            BaseAcceptanceTest.FrontEndServer.Dispose();
            try
            {
                SeleniumDriverManager.Driver?.Dispose();
                SeleniumDriverManager.Driver?.Quit();
            }
            catch
            {
            }
        }
    }
}
