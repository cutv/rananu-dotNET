using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Dev.Extensions
{
    public static class JavaScriptExecutorExtensions
    {

        public static bool ExecuteScriptToBoolean(this IJavaScriptExecutor jsExecutor, string js)
        {
            try
            {
                object result = jsExecutor.ExecuteScript(js);
                return result != null ? (bool)result : false;
            }
            catch (Exception) { return false; }
        }
        public static void QSelectorClick(this IJavaScriptExecutor jsExecutor, string selectorXpath)
        {
            jsExecutor.ExecuteScript($"var e=document.querySelector('{selectorXpath}');if(e!==null){{e.click(); console.log(\"querySelector click\");}}");
        }
        public static void Click(this IJavaScriptExecutor jsExecutor, string xpath)
        {
            jsExecutor.ExecuteScript($"var e=document.evaluate('{xpath}',document,null,XPathResult.ANY_TYPE,null).iterateNext();if(e!==null){{e.click(); console.log(\"evaluate click\");}}");
        }
        public static bool ElementExist(this IJavaScriptExecutor jsExecutor, string xpath)
        {
            return (bool)jsExecutor.ExecuteScript($"return null !== document.evaluate('{xpath}',document,null,XPathResult.ANY_TYPE,null).iterateNext();");
        }
        public static void ClickOnParentElement(this IJavaScriptExecutor jsExecutor, string xpath)
        {
            jsExecutor.ExecuteScript($"var e=document.evaluate('{xpath}',document,null,XPathResult.ANY_TYPE,null).iterateNext();if(e!==null)e.parentElement.click();");
        }
        public static void DisableChromeWebRTC(this IJavaScriptExecutor jsExecutor)
        {
            InfoText(jsExecutor, "Disable WebRTC");
            var js = File.ReadAllText("WebExtensions\\Chrome\\WebRTC.js");
            jsExecutor.ExecuteScript(js);
        }
        public static void InfoText(this IJavaScriptExecutor jsExecutor, string text)
        {
            InfoJS(jsExecutor, $"\"{text}\"");
        }
        public static void InfoJS(this IJavaScriptExecutor jsExecutor, string js)
        {
            try
            {
                jsExecutor.ExecuteScript($"console.info({js});console.info(new Date());");
            }
            catch (Exception ex)
            {
                jsExecutor.ExecuteScript($"console.error(\"{ex.Message}\")");
            }

        }
        public static void ErrorText(this IJavaScriptExecutor jsExecutor, string text)
        {
            ErrorJS(jsExecutor, $"\"{text}\"");

        }
        public static void ErrorJS(this IJavaScriptExecutor jsExecutor, string js)
        {
            try
            {
                jsExecutor.ExecuteScript($"console.error({js});console.info(new Date());");
            }
            catch (Exception ex)
            {
                jsExecutor.ExecuteScript($"console.error(\"{ex.Message}\")");
            }

        }
    }
}
