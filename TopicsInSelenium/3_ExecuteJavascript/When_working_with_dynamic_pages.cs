using System.Linq;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace TopicsInSelenium._3_ExecuteJavascript
{
    [TestFixture]
    public class When_working_with_dynamic_pages
    {
        [Test]
        public void Example_ExecuteJavaScript()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("https://duckduckgo.com/");
                driver.FindElement(By.XPath("//*[contains(@class, 'ddgst-more')]")).Click();

               
                driver.ExecuteJavaScript("document.getElementById('learn_more_modal').style.display='none'");

                // Set a breakpoint here
                driver.ExecuteJavaScript("document.getElementById('learn_more_modal').style.display='block'");

                // Set another breakpoint after this line. You can use the Javascript console to try to figure out what you need to run.
                // The example above is contrived. This method is most useful when a.) waiting for an element to finish animating (e.g.  or 
                // b.) the UI element you're trying to interact with is hidden
            }
        }
    }

    public static class WebDriverExtensions
    {
        public static object ExecuteJavaScript(this IWebDriver driver, string js)
        {
            return ((IJavaScriptExecutor)driver).ExecuteScript(js, new object[1]
              {
                (object) Enumerable.Empty<object>()
              });
        }
    }
}