using Bing_Rewards.Dashboard;
using Bing_Rewards.Utilities;
using General;
using General.Json;
using General.Json.Linq;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bing_Rewards.Account
{
    public class RewardAccount
    {
        public event EventHandler<EventArgs>? LoadSucceeded;
        public event EventHandler<EventArgs>? LoadFailed;

        public bool IsLogin { get; set; }
        public string? UserName { get; set; }

        [JsonProperty]
        private Cookie[]? all_cookie;
        private CookieContainer _cookies;
        [JsonProperty]
        private string? client_info;
        [JsonProperty]
        private string? code;
        [JsonProperty]
        private string? state;

        public RewardAccount()
        {
            _cookies = new();
        }

        public void LoadAllCookie()
        {
            if (all_cookie != null)
            {
                for (int i = 0; i < all_cookie.Length; i++)
                {
                    _cookies.Add(all_cookie[i]);
                }
            }
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
                string? responseString = await httpClient.GetResponseString(locationHeader);
                if (string.IsNullOrEmpty(responseString))
                {
                    return false;
                }
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
            try
            {
                await httpClient.PostAsync(url, encodedContent);
            }
            catch { }
            all_cookie = _cookies.GetAllCookies().ToArray();
        }

        public async Task<string?> GetuserInfo()
        {
            Uri uri = new($"https://rewards.bing.com/api/getuserinfo?type=5&X-Requested-With=XMLHttpRequest&_={TimeUtility.GetTimeStamp()}");
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Referrer = new("https://rewards.bing.com/");
            httpClient.SetUserAgent();
            string? responseString = await httpClient.GetResponseString(uri);
            if (string.IsNullOrEmpty(responseString))
            {
                return null;
            }
            string xpath = "/html/head/title";
            HtmlDocument document = new();
            document.LoadHtml(responseString);
            HtmlNode? titleNode = document.DocumentNode.SelectSingleNode(xpath);
            if (titleNode != null)
            {
                IsLogin = false;
                UserName = null;
                return null;
            }
            IsLogin = true;
            UserName = await GetUserName();
            return responseString;
        }

        public async Task SearchFromPC(string q)
        {
            try
            {
                string bingSearchUrl = "https://bing.com/search?q=" + q;
                Uri uri = new(bingSearchUrl);
                HttpClientHandler handler = new() { CookieContainer = _cookies };
                using HttpClient httpClient = new(handler);
                httpClient.SetUserAgent();
                string? html = await httpClient.GetResponseString(uri);
                if (string.IsNullOrEmpty(html))
                {
                    return;
                }
                string IG = Regex.Match(html, @"IG:""(.*?)""").Groups[1].Value;

                httpClient.AddDefaultHeadsWithNess();
                Dictionary<string, string> formParameters = new()
                {
                    { "wb", "1" },
                    { "i", "1" },
                    { "v", "1" }
                };
                FormUrlEncodedContent formEncodedContent = new(formParameters);
                await httpClient.PostAsync($"https://www.bing.com/rewardsapp/ncheader?&IID=SERP.5053&IG={IG}", formEncodedContent);

                string rewardUrl = $"https://bing.com/rewardsapp/reportActivity?IG={IG}&IID=SERP.5054&q={q}";
                uri = new(rewardUrl);
                Dictionary<string, string> parameters = new()
                {
                    { "url", bingSearchUrl },
                    { "V", "web" }
                };
                FormUrlEncodedContent encodedContent = new(parameters);
                httpClient.DefaultRequestHeaders.Referrer = new(bingSearchUrl);
                await httpClient.PostAsync(uri, encodedContent);
            }
            catch { }
        }

        public async Task SearchFromMobile(string q)
        {
            try
            {
                string bingSearchUrl = "https://bing.com/search?q=" + q;
                Uri uri = new(bingSearchUrl);
                HttpClientHandler handler = new() { CookieContainer = _cookies };
                using HttpClient httpClient = new(handler);
                httpClient.DefaultRequestHeaders.Referrer = new(bingSearchUrl);
                httpClient.SetUserAgentWithMobile();
                string? html = await httpClient.GetResponseString(uri);
                if (string.IsNullOrEmpty(html))
                {
                    return;
                }
                string ig = Regex.Match(html, @"IG:""(.*?)""").Groups[1].Value;

                string rewardUrl = $"https://bing.com/rewardsapp/reportActivity?IG={ig}&IID=SERP.6000.5395&q={q}&qs=LT&sk=PRES1&sc=10-2&FORM=QBRE&sp=1&lq=0";
                uri = new(rewardUrl);
                Dictionary<string, string> parameters = new()
                {
                    { "url", bingSearchUrl },
                    { "V", "web" }
                };
                FormUrlEncodedContent encodedContent = new(parameters);
                httpClient.DefaultRequestHeaders.Referrer = new(bingSearchUrl);
                await httpClient.PostAsync(uri, encodedContent);
            }
            catch { }
        }

        public async Task<CompleteDashboard> GetComplete()
        {
            CompleteDashboard completeDashboard = new();
            try
            {
                JObject stateObj = JObject.Parse(await GetuserInfo());
                bool complete0 = (bool)stateObj["status"]["userStatus"]["counters"]["pcSearch"][0]["complete"];
                bool complete1 = (bool)stateObj["status"]["userStatus"]["counters"]["pcSearch"][1]["complete"];
                completeDashboard.PC = complete0 && complete1;
                bool complete = (bool)stateObj["status"]["userStatus"]["counters"]["mobileSearch"][0]["complete"];
                completeDashboard.Mobile = complete;
            }
            catch { }
            return completeDashboard;
        }

        public async Task<string> GetUserName()
        {
            HttpClientHandler handler = new() { CookieContainer = _cookies };
            using HttpClient httpClient = new(handler);
            httpClient.DefaultRequestHeaders.Referrer = new("https://rewards.bing.com/");
            httpClient.SetUserAgent();
            string? html = await httpClient.GetResponseString(new("https://rewards.bing.com/"));
            if (string.IsNullOrEmpty(html))
            {
                return "Unknown";
            }
            string xpath = "//*[@id=\"mectrl_currentAccount_primary\"]";
            HtmlDocument document = new();
            document.LoadHtml(html);
            HtmlNode? nameNode = document.DocumentNode.SelectSingleNode(xpath);
            if (nameNode == null)
            {
                return "Unknown";
            }
            else
            {
                return nameNode.InnerText;
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

        public static RewardAccount[] LoadAccounts(string path)
        {
            List<RewardAccount> accounts = new();
            if (!File.Exists(path))
            {
                return accounts.ToArray();
            }
            string json = File.ReadAllText(path);
            accounts = JsonConvert.DeserializeObject<List<RewardAccount>>(json);
            for (int i = 0; i < accounts.Count; i++)
            {
                accounts[i].LoadAllCookie();
            }
            return accounts.ToArray();
        }

        public static void SaveAccounts(string path, RewardAccount[] accounts)
        {
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(path, json);
        }
    }
}
