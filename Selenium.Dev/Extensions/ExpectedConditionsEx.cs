using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpectedConditionsExtras = SeleniumExtras.WaitHelpers.ExpectedConditions;
namespace Selenium.Dev.Extensions
{
    public static class ExpectedConditionsEx
    {
        public static Func<IWebDriver, bool> ScriptExcuteToBoolean(string script)
        {
            return driver =>
            {
                try
                {
                    object result = (driver as IJavaScriptExecutor).ExecuteScript(script);
                    return result != null ? (bool)result : false;
                }
                catch (Exception) { return false; }
            };
        }
        public static Func<IWebDriver, bool> DocumentStateIsComplete()
        {
            return driver =>
            {
                try
                {
                    object result = (driver as IJavaScriptExecutor).ExecuteScript("return document.readyState==='complete'");
                    return result != null ? (bool)result : false;
                }
                catch (Exception) { return false; }

            };
        }

        public static Func<IWebDriver, string> ScriptExcuteToString(long millisExpired, string script)
        {
            return driver =>
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return "exprired";
                object result = (driver as IJavaScriptExecutor).ExecuteScript(script);
                return (string)result;
            };
        }

        public static Func<IWebDriver, bool> ScriptExcuteToBoolean(string script,  long millisExpired)
        {
            return driver =>
            {
                try
                {
                    if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                        return false;
                    object result = (driver as IJavaScriptExecutor).ExecuteScript(script);
                    return result != null ? (bool)result : false;
                }
                catch (Exception) { return false; }
            };
        }
        public static Func<IWebDriver, bool> ElementsExists(By locator, int size)
        {
            return driver =>
            {
                var elements = driver.FindElements(locator);
                return elements.Count() >= size;
            };
        }

        public static Func<IWebDriver, bool> Expired(long millisExpired)
        {
            return driver =>
            {
                return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired;
            };
        }

        public static Func<IWebDriver, Tuple<IWebElement>> ElementIsVisibleExpired(By locator, long millisExpired)
        {
            return driver =>
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return new Tuple<IWebElement>(null);
                try
                {
                    var func = ExpectedConditionsExtras.ElementIsVisible(locator);
                    var element = func(driver);
                    if (element != null)
                        return new Tuple<IWebElement>(element);
                }
                catch (Exception) { }
                return null;
            };
        }

        public static Func<IWebDriver, Object> ElementIsVisible(long millisExpired, By invalid, By valid)
        {
            return driver =>
            {

                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return -1;
                try
                {
                    Func<IWebDriver, IWebElement> func = ExpectedConditionsExtras.ElementIsVisible(invalid);
                    if (func(driver) != null)
                        return 0;
                }
                catch (Exception) { }
                try
                {
                    Func<IWebDriver, IWebElement> func = ExpectedConditionsExtras.ElementIsVisible(valid);
                    if (func(driver) != null)
                        return 1;
                }
                catch (Exception) { }
                return null;
            };
        }

        public static Func<IWebDriver, Tuple<IWebElement>> ElementToBeClickable(By locator, long millisExpired)
        {
            return (driver) =>
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return new Tuple<IWebElement>(null);
                try
                {
                    var func = ExpectedConditionsExtras.ElementToBeClickable(locator);
                    var element = func(driver);
                    if (element != null) return new Tuple<IWebElement>(element);
                }
                catch (Exception) { }
                return null;
            };
        }
        public static Func<IWebDriver, Object> UrlToBe(string url, long millisExpired)
        {
            return (driver) =>
            {
                try
                {
                    if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                        return -1;
                    var func = ExpectedConditionsExtras.UrlToBe(url);
                    if (func(driver))
                        return 1;
                }
                catch (Exception) { }
                return null;
            };
        }

        public static Func<IWebDriver, int?> ElementsIsVisible(long millisExpired, By[] invalids, params By[] valids)
        {
            return driver =>
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return -1;
                IWebElement element = null;
                foreach (var item in valids)
                {
                    try
                    {
                        element = driver.FindElement(item);
                        if (element != null)
                            return 1;
                    }
                    catch (Exception) { }
                }
                foreach (var item in invalids)
                {
                    try
                    {
                        element = driver.FindElement(item);
                        if (element != null)
                            return 0;
                    }
                    catch (Exception) { }
                }

                return null;
            };
        }

        public static Func<IWebDriver, string> ElementsIsType(long millisExpired, params By[] locatorTypes)
        {
            return driver =>
            {
                if (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > millisExpired)
                    return "exprired";
                IWebElement element = null;
                foreach (var item in locatorTypes)
                {
                    try
                    {
                        element = driver.FindElement(item);
                        if (element != null)
                            return "valid";
                    }
                    catch (Exception) { }
                }

                return null;
            };
        }


    }
}
