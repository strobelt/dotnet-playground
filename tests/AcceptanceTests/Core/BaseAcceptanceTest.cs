using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

namespace AcceptanceTests.Core
{
    public class BaseAcceptanceTest
    {
        public static NpmScript FrontEndServer { get; set; }
        public static string FrontEndUrl => FrontEndServer?.Url;
        public static IWebHost BackEndServer { get; set; }
        public static HttpClient BackEndClient { get; set; }
        public static string BackEndUrl { get; set; }
    }
}
