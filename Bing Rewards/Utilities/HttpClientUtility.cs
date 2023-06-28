using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bing_Rewards.Utilities
{
    public static class HttpClientUtility
    {
        public static CookieCollection GetAllCookies(this CookieContainer cookieContainer)
        {
            CookieCollection cookieCollection = new();
            Hashtable? table = (Hashtable?)cookieContainer.GetType()?.InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cookieContainer, Array.Empty<object>());
            foreach (object pathList in table!.Values)
            {
                SortedList<string, CookieCollection>? lstCookieCol = (SortedList<string, CookieCollection>?)pathList.GetType()?.InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, Array.Empty<object>());
                foreach (CookieCollection cookieCollection1 in lstCookieCol!.Values)
                {
                    cookieCollection.Add(cookieCollection1);
                }
            }
            return cookieCollection;
        }

        public static void AddCookies(this CookieContainer cookieContainer, CookieCollection cookies)
        {
            foreach (Cookie cookie in cookies.Cast<Cookie>())
            {
                cookieContainer.Add(cookie);
            }
        }

        public static void SetUserAgent(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.UserAgent.Clear();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36 Edg/116.0.0.0");
        }
        public static void SetUserAgentWithMobile(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.UserAgent.Clear();
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Mobile Safari/537.36 Edg/114.0.1823.58");
        }

        public static void AddDefaultHeadsWithNess(this HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"112\", \"Microsoft Edge\";v=\"112\", \"Not:A-Brand\";v=\"99\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-arch", "\"x86\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-bitness", "\"64\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-full-version", "\"112.0.1722.23\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-full-version-list", "\"Chromium\";v=\"112.0.5615.39\", \"Microsoft Edge\";v=\"112.0.1722.23\", \"Not:A-Brand\";v=\"99.0.0.0\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-model", "");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
            httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform-version", "\"16.0.0\"");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
            httpClient.DefaultRequestHeaders.Add("sec-fetch-site", "same-origin");
        }

        public static async Task<string?> GetResponseString(this HttpClient httpClient, Uri uri)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string?> PostResponseString(this HttpClient httpClient, Uri uri, FormUrlEncodedContent encodedContent)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(uri, encodedContent);
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<string?> PostResponseString(this HttpClient httpClient, Uri uri, MultipartFormDataContent multipartFormDataContent)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(uri, multipartFormDataContent);
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            catch
            {
                return null;
            }
        }
    }
}
