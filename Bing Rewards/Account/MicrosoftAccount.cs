using Bing_Rewards.Utilities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bing_Rewards.Account
{
    public class MicrosoftAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }

        private CookieContainer _cookies { get; set; }

        public MicrosoftAccount(string email, string password)
        {
            Email = email;
            Password = password;
            _cookies = new();
        }

        public async Task Login()
        {
            Uri uri = new("https://login.live.com/ppsecure/post.srf");
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.SetUserAgent();;
            string html = await httpClient.GetResponseString(uri);
            string pattern = @"sFTTag:'<input type=""hidden"" name=""PPFT"" id=""(.*?)"" value=""(.*?)""/>'";
            string ppft = Regex.Match(html, pattern).Groups[2].Value;

            Dictionary<string, string?> parameters = new()
            {
                { "login", Email },
                { "passwd", Password },
                { "PPFT", ppft }
            };
            FormUrlEncodedContent encodedContent = new(parameters);
            string responseString = await httpClient.PostResponseString(uri, encodedContent);
        }

        public CookieCollection GetCookies()
        {
            CookieCollection cookies = _cookies.GetAllCookies();
            return cookies;
        }
    }
}
