using OpenQA.Selenium;

namespace AcceptanceTests.Core
{
    public abstract class Page
    {
        public IWebDriver Driver = SeleniumDriverManager.Driver;
        public static string BaseUrl { get; set; }
        public abstract string Url { get; }
        public string CurrentUrl => Driver.Url;
        public void Navigate()
        {
            Driver.Url = $"{BaseUrl}{Url}";
            Driver.Navigate();
        }
    }
}
