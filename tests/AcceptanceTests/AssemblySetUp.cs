using AcceptanceTests.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.PlatformAbstractions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi;

namespace AcceptanceTests
{
    [SetUpFixture]
    public class AssemblySetUp
    {
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            var integrationTestsPath = PlatformServices.Default.Application.ApplicationBasePath;
            var applicationPath =
                Path.GetFullPath(Path.Combine(integrationTestsPath, "../../../../../src"));
            BaseAcceptanceTest.BackEndServer = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(applicationPath)
                .UseEnvironment("Development")
                .Build();

            await BaseAcceptanceTest.BackEndServer.StartAsync();
            BaseAcceptanceTest.BackEndUrl = BaseAcceptanceTest.BackEndServer.ServerFeatures
                .Get<IServerAddressesFeature>().Addresses.FirstOrDefault();

            BaseAcceptanceTest.BackEndClient =
                new HttpClient
                {
                    BaseAddress = new Uri(BaseAcceptanceTest.BackEndUrl)
                };

            BaseAcceptanceTest.FrontEndServer = new NpmScript("start");
            await BaseAcceptanceTest.FrontEndServer.RunAsync(x => Debug.WriteLine(x));
            Page.BaseUrl = BaseAcceptanceTest.FrontEndServer.Url;

            SeleniumDriverManager.Start();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            if (BaseAcceptanceTest.FrontEndServer != null)
                BaseAcceptanceTest.FrontEndServer.Dispose();

            if (SeleniumDriverManager.Driver != null)
                try
                {
                    SeleniumDriverManager.Driver.Quit();
                    SeleniumDriverManager.Driver.Dispose();
                }
                catch
                {
                }

            if (BaseAcceptanceTest.BackEndServer != null)
                try
                {
                    await BaseAcceptanceTest.BackEndServer.StopAsync();
                    BaseAcceptanceTest.BackEndServer.Dispose();
                }
                catch
                {
                }
        }
    }
}
