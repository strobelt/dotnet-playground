using AcceptanceTests.Core;
using OpenQA.Selenium;

namespace AcceptanceTests.Pages
{
    public class HomePage : Page
    {
        public override string Url => "/";
        public string Header => Driver.FindElement(By.TagName("h1")).Text;
    }
}
