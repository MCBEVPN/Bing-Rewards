using Bing_Rewards.Utilities;
using General.Json;
using General.Json.Linq;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bing_Rewards.Account
{
    public class RewardAccount
    {
        public event EventHandler<EventArgs>? LoadSucceeded;
        public event EventHandler<EventArgs>? LoadFailed;

        private CookieContainer _cookies;
        private string? client_info;
        private string? code;
        private string? state;

        public RewardAccount()
        {
            _cookies = new();
        }

        public async Task<bool> Login()
        {
            Uri uri = new("https://rewards.bing.com");
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.SetUserAgent();
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            Uri? locationHeader = response.RequestMessage?.RequestUri;
            if (locationHeader != null)
            {
                string responseString = await httpClient.GetResponseString(locationHeader);
                HtmlDocument document = new();
                document.LoadHtml(responseString);
                client_info = document.DocumentNode.SelectSingleNode("//*[@id=\"client_info\"]")?.Attributes["value"].Value;
                code = document.DocumentNode.SelectSingleNode("//*[@id=\"code\"]")?.Attributes["value"].Value;
                state = document.DocumentNode.SelectSingleNode("//*[@id=\"state\"]")?.Attributes["value"].Value;
                await LoginToRewardsAccount();
            }
            return !string.IsNullOrEmpty(client_info) && !string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state);
        }

        private async Task LoginToRewardsAccount()
        {
            if (client_info == null || code == null || state == null)
            {
                return;
            }
            Uri url = new("https://rewards.bing.com/signin-oidc");
            HttpClientHandler httpClientHandler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(httpClientHandler);
            httpClient.DefaultRequestHeaders.Add("Referer", "https://login.live.com/");
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            httpClient.SetUserAgent();
            Dictionary<string, string> parameters = new()
            {
                { "client_info", client_info },
                { "code", code },
                { "state", state }
            };
            FormUrlEncodedContent encodedContent = new(parameters);
            await httpClient.PostResponseString(url, encodedContent);
        }

        public async Task<string?> GetuserInfo()
        {
            Uri uri = new($"https://rewards.bing.com/api/getuserinfo?type=5&X-Requested-With=XMLHttpRequest&_={TimeUtility.GetTimeStamp()}");
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Referrer = new("https://rewards.bing.com/");
            httpClient.SetUserAgent();
            string responseString = await httpClient.GetResponseString(uri);
            string xpath = "/html/head/title";
            HtmlDocument document = new();
            document.LoadHtml(responseString);
            HtmlNode? titleNode = document.DocumentNode.SelectSingleNode(xpath);
            if (titleNode != null)
            {
                return null;
            }
            return responseString;
        }

        public async Task<bool> SearchFromPC(string q)
        {
            if (await CheckPCComplete())
            {
                return true;
            }
            string bingSearchUrl = "https://bing.com/search?q=" + q;
            Uri uri = new(bingSearchUrl);
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Referrer = new("https://bing.com/");
            httpClient.SetUserAgent();
            string html = await httpClient.GetResponseString(uri);
            string IG = Regex.Match(html, @"IG:""(.*?)""").Groups[1].Value;

            string rewardUrl = $"https://bing.com/rewardsapp/reportActivity?IG={IG}&IID=SERP.5054&q=%E4%BB%8A%E5%A4%A9&aqs=edge..69i57.9532j0j4&FORM=ANCMS9&PC=U531";
            uri = new(rewardUrl);
            Dictionary<string, string> parameters = new()
            {
                { "url", rewardUrl },
                { "V", "web" }
            };
            FormUrlEncodedContent encodedContent = new(parameters);
            httpClient.DefaultRequestHeaders.Referrer = new(rewardUrl);
            await httpClient.PostAsync(uri, encodedContent);
            return false;
        }

        public async Task<bool> SearchFromMobile(string q)
        {
            if (await CheckMobileComplete())
            {
                return true;
            }
            string bingSearchUrl = "https://bing.com/search?q=" + q;
            Uri uri = new(bingSearchUrl);
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Referrer = new("https://bing.com/");
            httpClient.SetUserAgentWithMobile();
            string html = await httpClient.GetResponseString(uri);
            string ig = Regex.Match(html, @"IG:""(.*?)""").Groups[1].Value;

            string rewardUrl = $"https://bing.com/rewardsapp/reportActivity?IG={ig}&IID=SERP.6000.5395&q={q}&qs=LT&sk=PRES1&sc=10-2&FORM=QBRE&sp=1&lq=0";
            uri = new(rewardUrl);
            Dictionary<string, string> parameters = new()
            {
                { "url", rewardUrl },
                { "V", "web" }
            };
            FormUrlEncodedContent encodedContent = new(parameters);
            httpClient.DefaultRequestHeaders.Referrer = new(rewardUrl);
            await httpClient.PostAsync(uri, encodedContent);
            return false;
        }

        private async Task<bool> CheckMobileComplete()
        {
            try
            {
                JObject stateObj = JObject.Parse(await GetuserInfo());
                bool complete = (bool)stateObj["status"]["userStatus"]["counters"]["mobileSearch"][0]["complete"];
                return complete;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> CheckPCComplete()
        {
            try
            {
                JObject stateObj = JObject.Parse(await GetuserInfo());
                bool complete0 = (bool)stateObj["status"]["userStatus"]["counters"]["pcSearch"][0]["complete"];
                bool complete1 = (bool)stateObj["status"]["userStatus"]["counters"]["pcSearch"][1]["complete"];
                return complete0 && complete1;
            }
            catch
            {
                return false;
            }
        }

        public void AddCookies(CookieCollection cookies)
        {
            foreach (Cookie cookie in cookies.Cast<Cookie>())
            {
                _cookies.Add(cookie);
            }
        }

        public CookieCollection GetCookies()
        {
            CookieCollection cookies = _cookies.GetAllCookies();
            return cookies;
        }

        public void SaveCookies(string path)
        {
            CookieCollection cookies = _cookies.GetAllCookies();
            CookieUtility.SaveCookies(cookies, path);
        }

        public async void LoadCookies(string path)
        {
            _cookies = new();
            CookieCollection cookies = CookieUtility.LoadCookies(path);
            AddCookies(cookies);
            string? userInfo = await GetuserInfo();
            if (userInfo == null)
            {
                LoadFailed?.Invoke(this, new());
            }
            else
            {
                LoadSucceeded?.Invoke(this, new());
            }
        }
    }
}
