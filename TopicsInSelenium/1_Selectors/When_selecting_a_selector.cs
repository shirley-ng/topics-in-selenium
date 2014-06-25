using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace TopicsInSelenium._1_Selectors
{
    [TestFixture]
    public class When_selecting_a_selector
    {
        [Test]
        public void Example_FragileXPath()
        {
            using (var driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("https://duckduckgo.com/");

                driver.FindElement(By.Id("search_form_input_homepage")).SendKeys("ecommerce software");
                driver.FindElement(By.Id("search_button_homepage")).Click();

                driver.FindElement(By.XPath("//*[@id=\"r1-2\"]/div/div[2]/div/a/span[1]")).Click();

                // Note in C#, in the XPath expression you will need to "escape" certain characters. See reference @ http://msdn.microsoft.com/en-us/library/h21280bw.aspx
                // In the above XPath, we had to escape the double quotation mark, e.g. "\"test\""

                Assert.That(driver.Url, Is.EqualTo("https://www.volusion.com/"));
            } 
        }

        [Test]
        public void Example_XPathFunctions()
        {
            using (var driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("https://duckduckgo.com/");

                // starts-with(string, string)
                driver.FindElement(By.XPath("//a[starts-with(@class, 'header__button--menu')]")).Click();

                // contains(string, string)
                driver.FindElement(By.XPath("//i[contains(@class, 'ddgst-more')]")).Click();

                // See W3C reference on XPath String Functions @ http://www.w3schools.com/xpath/xpath_functions.asp#string
            }
        }

        [Test]
        public void Example_ListOfMatches()
        {
            using (var driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("https://duckduckgo.com/");

                driver.FindElement(By.Id("search_form_input_homepage")).SendKeys("ecommerce software");
                driver.FindElement(By.Id("search_button_homepage")).Click();

                // This is a list of search result links from DuckDuckGo
                var searchResults = driver.FindElements(By.XPath("//div[@id='links']/div[contains(@class, 'results_links_deep')]"));
                IWebElement matchingSearchResult = null;
                foreach (IWebElement result in searchResults)
                {
                    // Check the title of each of the results. Note that we're using the "." selector here to select only children of the search result.
                    if (result.FindElement(By.XPath(".//h2[@class='result__title']")).Text == "Ecommerce Software & Shopping Cart Solutions by Volusion")
                    {
                        matchingSearchResult = result;
                    }
                }
                if (matchingSearchResult == null)
                {
                    throw new InvalidOperationException("Unable to locate search result matching Volusion");
                }
                matchingSearchResult.Click();

                Assert.That(driver.Url, Is.EqualTo("https://www.volusion.com/"));
            }
        }

        [Test]
        public void Example_ReusableFunctionForClickingMatch()
        {
            using (var driver = new FirefoxDriver())
            {
                driver.Navigate().GoToUrl("https://duckduckgo.com/");
                Search(driver, "ecommerce software");
                GoToSearchResult(driver, "Ecommerce Software & Shopping Cart Solutions by Volusion");

                Assert.That(driver.Url, Is.EqualTo("https://www.volusion.com/"));

                driver.Navigate().GoToUrl("https://duckduckgo.com/");
                Search(driver, "ecommerce software");
                GoToSearchResult(driver, "Bigcommerce - Ecommerce Software & Shopping Cart Solutions ...");

                Assert.That(driver.Url, Is.EqualTo("https://www.bigcommerce.com/"));
            }
        }

        private void Search(IWebDriver driver, string searchTerm)
        {
            driver.FindElement(By.Id("search_form_input_homepage")).SendKeys(searchTerm);
            driver.FindElement(By.Id("search_button_homepage")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.FindElements(By.XPath("//div[@id='links']/div[contains(@class, 'results_links_deep')]")));
        }

        private void GoToSearchResult(IWebDriver driver, string searchResultTitle)
        {
            IList<IWebElement> searchResults = driver.FindElements(By.XPath("//div[@id='links']/div[starts-with(@id, 'r1-')]"));
            
            /*IWebElement matchingSearchResult = null;
            foreach (IWebElement result in searchResults)
            {
                if (result.FindElement(By.XPath(".//h2[@class='result__title']")).Text == searchResultTitle)
                {
                    matchingSearchResult = result;
                }
            }*/

            // This is a lambda expression. It is syntactic sugar and is equivalent to the foreach loop above. See reference @ http://www.dotnetperls.com/lambda
            IWebElement matchingSearchResult = searchResults.FirstOrDefault(result => result.FindElement(By.XPath(".//h2[@class='result__title']")).Text == searchResultTitle);

            if (matchingSearchResult == null)
            {
                throw new InvalidOperationException(String.Format("Unable to locate search result matching {0}", searchResultTitle));
            }
            matchingSearchResult.Click();
        }
    }
}