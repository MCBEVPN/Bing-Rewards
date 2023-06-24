using General.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace Bing_Rewards.Utilities
{
    public class CookieUtility
    {
        public static void SaveCookies(CookieCollection cookies, string path)
        {
            string json = JsonConvert.SerializeObject(cookies);
            File.WriteAllText(path, json);
        }

        public static CookieCollection LoadCookies(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<CookieCollection>(json);
        }

        public static CookieContainer LoadCookiesContainer(string path)
        {
            CookieContainer container = new();
            CookieCollection cookies = LoadCookies(path);
            foreach (Cookie cookie in cookies.Cast<Cookie>())
            {
                container.Add(cookie);
            }
            return container;
        }
    }
}