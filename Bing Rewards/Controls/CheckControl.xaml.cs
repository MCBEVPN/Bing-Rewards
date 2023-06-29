using Bing_Rewards.Account;
using Bing_Rewards.Utilities;
using General.Json.Linq;
using HtmlAgilityPack;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bing_Rewards.Controls
{
    /// <summary>
    /// QueryControl.xaml 的交互逻辑
    /// </summary>
    public partial class CheckControl : UserControl
    {
        public RewardAccount? Account { get; set; }
        public CheckControl(RewardAccount? rewardAccount)
        {
            Account = rewardAccount;
            InitializeComponent();
        }

        public CheckControl()
        {
            InitializeComponent();
        }

        public async Task Initialize()
        {
            if (Account == null)
            {
                SetText("目前不可用", true);
            }
            else
            {
                string? html = await GetAccountContentAsync();
                if (html == null)
                {
                    SetText("验证完成：网络出错了", true);
                }
                else
                {
                    HtmlDocument htmlDocument = new();
                    htmlDocument.LoadHtml(html);

                    HtmlNode? headerNode = htmlDocument.DocumentNode.Descendants("span").FirstOrDefault(x => x.Id.Equals("id_n"));
                    if (headerNode == null)
                    {
                        SetText("验证完成：登录不成功", true);
                    }
                    else
                    {
                        SetText("验证完成：无可疑问题", false);
                    }
                }
            }
        }

        private async Task<string?> GetAccountContentAsync()
        {
            HttpClientHandler httpClientHandler = new();
            if (Account != null)
            {
                CookieContainer cookieContainer = new();
                cookieContainer.Add(Account.GetCookies());
                httpClientHandler.CookieContainer = cookieContainer;
            }
            HttpClient httpClient = new(httpClientHandler);
            httpClient.SetUserAgent();
            httpClient.AddDefaultHeadsWithNess();
            string? response = await httpClient.GetResponseString(new("https://bing.com"));
            return response;
        }

        private void SetText(string text, bool error)
        {
            this.checkTip.Text = text;
            defaultImg.Visibility = Visibility.Collapsed;
            if (error)
            {
                this.error.Visibility = Visibility.Visible;
                this.succeed.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.error.Visibility = Visibility.Collapsed;
                this.succeed.Visibility = Visibility.Visible;
            }
        }
    }
}
