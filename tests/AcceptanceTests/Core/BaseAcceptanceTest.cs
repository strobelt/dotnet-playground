namespace AcceptanceTests.Core
{
    public class BaseAcceptanceTest
    {
        public static NpmScript FrontEndServer { get; set; }
        public static string FrontEndUrl => FrontEndServer?.Url;
    }
}
