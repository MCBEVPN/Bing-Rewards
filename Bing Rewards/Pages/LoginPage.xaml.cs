using Bing_Rewards.Account;
using Bing_Rewards.Utilities;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Bing_Rewards.Pages
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public event EventHandler? LoginSucceeded;
        public event EventHandler? LoginFailed;

        public RewardAccount? RewardAccount { get; set; }

        private bool _IsLogin;
        public LoginPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            this.Loaded += delegate
            {
                if (!_IsLogin)
                {
                    PlayStartAnimation();
                }
            };
        }

        private async void PlayStartAnimation()
        {
            await mainBorder.SlideIn(500);
        }

        private async void PlayEndAnimation()
        {
           await mainBorder.SlideOut(500);
        }

        private async void PlayErrorAnimation()
        {
            await mainBorder.SlideBack(500);
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            mainBorder.IsEnabled = false;
            PlayEndAnimation();
            Login();
        }

        private async void Login()
        {
            _IsLogin = true;
            pb.Visibility = Visibility.Visible;
            BingAccount bingAccount = new(emailText.Text.Trim(), pwdText.Password.Trim());
            await bingAccount.Login();
            RewardAccount = new();
            RewardAccount.AddCookies(bingAccount.GetCookies());
            if (await RewardAccount.Login())
            {
                string? userInfo = await RewardAccount.GetuserInfo();
                if (userInfo != null)
                {
                    LoginSucceeded?.Invoke(this, new());
                }
                else
                {
                    errorText.Text = "请检查网络是否正常，如正常则账号或密码不正确。";
                    LoginFailed?.Invoke(this, new());
                }
            }
            else
            {
                PlayErrorAnimation();
                errorText.Text = "请检查网络是否正常，如正常则账号或密码不正确。";
                mainBorder.IsEnabled = true;
                LoginFailed?.Invoke(this, new());
            }
            pb.Visibility = Visibility.Collapsed;
            _IsLogin = false;
        }

        private void PwdText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginClick(sender, e);
            }
        }
    }
}
