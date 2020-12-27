using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AcceptanceTests.Core
{
    public static class SeleniumDriverManager
    {
        public static IWebDriver Driver { get; private set; }

        public static void Start(string seleniumServerAddress = null)
        {
            if (string.IsNullOrWhiteSpace(seleniumServerAddress))
                StartLocalDriver();
            else
                StartRemoteDriver(seleniumServerAddress);
        }

        private static void StartRemoteDriver(string seleniumServerAddress)
        {
            var chromeInitializationOptions = new ChromeOptions();
            chromeInitializationOptions.AddArgument("--headless");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                chromeInitializationOptions.AddArgument("--disable-gpu"); // until not needed, see https://developers.google.com/web/updates/2017/04/headless-chrome
            Driver = new RemoteWebDriver(new Uri(seleniumServerAddress), chromeInitializationOptions);
            Driver.Manage().Window.Maximize();
        }

        private static void StartLocalDriver()
        {
            var chromeInitializationOptions = new ChromeOptions();
            if (!Debugger.IsAttached)
            {
                chromeInitializationOptions.AddArgument("--headless");
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    chromeInitializationOptions.AddArgument("--disable-gpu"); // until not needed, see https://developers.google.com/web/updates/2017/04/headless-chrome
            }
            Driver = new ChromeDriver(Environment.CurrentDirectory, chromeInitializationOptions);
            Driver.Manage().Window.Maximize();
        }
    }
}
