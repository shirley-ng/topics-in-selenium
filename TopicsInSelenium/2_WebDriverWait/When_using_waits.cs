using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TopicsInSelenium._2_WebDriverWait
{
    [TestFixture]
    public class When_using_waits
    {
        [Test]
        public void Example_Basic()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                webDriverWait.Until(ExpectedConditions.ElementExists(By.XPath("")));
                webDriverWait.Until(ExpectedConditions.ElementIsVisible(By.Id("")));
                webDriverWait.Until(ExpectedConditions.TitleContains("Test"));
                webDriverWait.Until(ExpectedConditions.TitleContains("Testing"));
            } 
        }

        [Test]
        public void Example_Polling()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                var webDriverWait = new WebDriverWait(new SystemClock(), driver, TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(5));

                // Not used often but specific circumstances warrant this. e.g. in current state Storefront, you can only submit 1 order per minute. 
                // UI Automation tests running from CI move faster than this so we put in a longer wait and a 5 second polling frequency. Default poll time is 500ms.
            }
        }

        [Test]
        public void Example_CustomWaitCondition()
        {
            using (IWebDriver driver = new FirefoxDriver())
            {
                // Passing in a function that takes a WebDriver and returns a bool
                var webDriverWait1 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                webDriverWait1.Until(PageIsDoneLoading);

                // Passing in an anonymous function that takes a WebDriver and returns a bool
                var webDriverWait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                webDriverWait2.Until(d => d.Title == "Account Management");

                // Old style syntax for anonymous function
                var webDriverWait3 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                webDriverWait3.Until(delegate(IWebDriver d) { return d.Title == "Account Management"; });

                // Longer anonymous function that takes a WebDriver and returns a bool
                var webDriverWait4 = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                webDriverWait4.Until(d =>
                {
                    if (d.Title == "Account Management")
                    {
                        return true;
                    }
                    return false;
                });
            }
        }

        private bool PageIsDoneLoading(IWebDriver driver)
        {
            // This can be as complicated as needed
            return driver.Title == "Account Management";
        }

        private void MethodThatTakesADelegate(IWebDriver driver, Func<IWebDriver, bool> isDoneWaiting)
        {
            if (isDoneWaiting(driver))
            {
                // Stop waiting!
                return;
            }
            else
            {
                // Keep waiting
            }
        }
    }
}