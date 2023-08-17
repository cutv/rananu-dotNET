using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Dev.Extensions
{
    public static class WebElementExtensions
    {
        public static IWebElement FindElementCatchException(this ISearchContext driver, By locator)
        {
            try
            {
                return driver.FindElement(locator);
            }
            catch (NoSuchElementException) { }
            catch (StaleElementReferenceException) { }
            return null;
        }
    }
}
