using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bing_Rewards.Utilities;
using System.IO;

namespace Bing_Rewards.Account
{
    public class BingAccount : MicrosoftAccount
    {
        public BingAccount(string email, string password) : base(email, password)
        {
        }

        public new async Task Login()
        {
            await base.Login();
            await LoginToBing();
        }

        private async Task LoginToBing()
        {
            try
            {
                HttpClientHandler handler = new() { CookieContainer = _cookies };
                using HttpClient httpClient = new(handler);
                httpClient.SetUserAgent();
                string html = await httpClient.GetStringAsync("https://bing.com/fd/auth/signin?action=interactive&provider=windows_live_id&return_url=https://bing.com/?wlexpsignin=1&wlexpsignin=1&src=EXPLICIT");
                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(html);
                string? pprid = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"pprid\"]")?.GetAttributeValue("value", "");
                string? nap = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"NAP\"]")?.GetAttributeValue("value", "");
                string? anon = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"ANON\"]")?.GetAttributeValue("value", "");
                string? t = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"t\"]")?.GetAttributeValue("value", "");
                string? url = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"fmHF\"]")?.GetAttributeValue("action", "");
                if (!string.IsNullOrEmpty(url))
                {
                    string content = $"NAPExp={DateTime.Now.ToUniversalTime():R}" +
                        $"&pprid={pprid}" +
                        $"&NAP={nap}" +
                        $"&ANON={anon}" +
                        $"&ANONExp={DateTime.Now.AddMonths(7).ToUniversalTime():R}" +
                        $"&t={t}";
                    StringContent stringContent = new(content);
                    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

                    httpClient.DefaultRequestHeaders.Referrer = new Uri("https://login.live.com/");
                    await httpClient.PostAsync(url, stringContent).ConfigureAwait(false);
                }
            }
            catch { }
        }
    }
}
