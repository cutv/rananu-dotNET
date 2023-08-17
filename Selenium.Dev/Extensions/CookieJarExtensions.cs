using Common;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Dev.Extensions
{
    public static class CookieJarExtensions
    {
        public static void AddAll(this ICookieJar cookieJar, IEnumerable<Cookie> cookies)
        {
            cookieJar.DeleteAllCookies();
            foreach (var cookie in cookies)
            {
                cookieJar.AddCookie(cookie);
            }
        }
        public static void LoadCookie(this ICookieJar cookieJar, string cookieFilePath, string domain = null)
        {
            string json = File.ReadAllText(cookieFilePath);
            IEnumerable<JsonCookie> cookieList = NewtonsoftJsonConvert.Instance.DeserializeObject<IEnumerable<JsonCookie>>(json);
            if (domain != null)
                foreach (var cookie in cookieList)
                {
                    cookie.Domain = domain;
                }
            cookieJar.DeleteAllCookies();
            foreach (JsonCookie cookie in cookieList)
                cookieJar.AddCookie(cookie.ToCookie());
        }
        public static void LoadCookie(this ICookieJar cookieJar, string cookieFilePath)
        {
            string json = File.ReadAllText(cookieFilePath);
            IEnumerable<JsonCookie> cookieList = NewtonsoftJsonConvert.Instance.DeserializeObject<IEnumerable<JsonCookie>>(json);
            foreach (JsonCookie cookie in cookieList)
                cookieJar.AddCookie(cookie.ToCookie());
        }
        public static void StoreCookie(this ICookieJar cookieJar, string cookieFilePath)
        {
            File.WriteAllText(cookieFilePath, NewtonsoftJsonConvert.Instance.SerializeObject(cookieJar.AllCookies));
        }
    }
}
