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

        public static async Task<string> GetResponseString(this HttpClient httpClient, Uri uri)
        {
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public static async Task<string> PostResponseString(this HttpClient httpClient, Uri uri, FormUrlEncodedContent encodedContent)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, encodedContent);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        public static async Task<string> PostResponseString(this HttpClient httpClient, Uri uri, MultipartFormDataContent multipartFormDataContent)
        {
            HttpResponseMessage response = await httpClient.PostAsync(uri, multipartFormDataContent);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
