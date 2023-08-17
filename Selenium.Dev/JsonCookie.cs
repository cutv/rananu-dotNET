using Common;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Dev
{
    public class JsonCookie
    {
        public JsonCookie()
        {
        }
        public JsonCookie(Cookie cookie)
        {
            if (cookie != null)
            {
                Name = cookie.Name;
                Value = cookie.Value;
                Domain = cookie.Domain;
                Path = cookie.Path;
                Secure = cookie.Secure;
                IsHttpOnly = cookie.IsHttpOnly;
                Expiry = cookie.Expiry;
            }

        }

        public Cookie ToCookie()
        {
            return new Cookie(Name, Value, Domain, Path, Expiry);
        }

        //
        // Summary:
        //     Gets the name of the cookie.
        [JsonProperty("name")]
        public string Name { get; set; }
        //
        // Summary:
        //     Gets the value of the cookie.
        [JsonProperty("value")]
        public string Value { get; set; }
        //
        // Summary:
        //     Gets the domain of the cookie.
        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }
        //
        // Summary:
        //     Gets the path of the cookie.
        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public virtual string Path { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the cookie is secure.
        [JsonProperty("secure")]
        public virtual bool Secure { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether the cookie is an HTTP-only cookie.
        [JsonProperty("httpOnly")]
        public virtual bool IsHttpOnly { get; set; }
        //
        // Summary:
        //     Gets the expiration date of the cookie.
        [JsonProperty("expiry")]
        public DateTime? Expiry { get; set; }

        public static void LoadCookie(ICookieJar cookieJar, string cookiePath)
        {
            if (File.Exists(cookiePath))
            {
                string json = File.ReadAllText(cookiePath);
                if (!String.IsNullOrEmpty(json))
                {
                    List<JsonCookie> cookieList = NewtonsoftJsonConvert.Instance.DeserializeObject<List<JsonCookie>>(json);
                    foreach (JsonCookie cookie in cookieList)
                        cookieJar.AddCookie(cookie.ToCookie());


                }
            }
        }

        public static void SaveCookie(ICookieJar cookieJar, string cookiePath)
        {
            FileInfo fileInfo = new FileInfo(cookiePath);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();
           /* if (!File.Exists(cookiePath))
                File.Create(cookiePath).Close();*/
            File.WriteAllText(cookiePath, NewtonsoftJsonConvert.Instance.SerializeObject(cookieJar.AllCookies));
        }
    }
}
