using AcceptanceTests.Core;
using OpenQA.Selenium;
using System.Collections.ObjectModel;

namespace AcceptanceTests.Pages
{
    public class WeatherPage : Page
    {
        public override string Url => "/weather";
        public string Header => Driver.FindElement(By.TagName("h2")).Text;

        public ReadOnlyCollection<IWebElement> ForecastCards =>
            Driver.FindElements(By.ClassName("forecast-card"), 5);
    }
}
