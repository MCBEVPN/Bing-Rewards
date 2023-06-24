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
        private const string COOKIE_PATH = "./user_cookies.json";
        public LoginPage()
        {
            InitializeComponent();
            PlayStartAnimation();
            InitializeCookies();
        }

        private void InitializeCookies()
        {
            if (File.Exists(COOKIE_PATH))
            {
                pb.Visibility = Visibility.Visible;
                mainBorder.IsEnabled = false;
                RewardAccount rewardAccount = new();
                rewardAccount.LoadCookies(COOKIE_PATH);
                rewardAccount.LoadFailed += delegate
                {
                    PlayErrorAnimation();
                    mainBorder.IsEnabled = true;
                };
                rewardAccount.LoadSucceeded += delegate
                {
                    pb.Visibility = Visibility.Collapsed;
                    rewardAccount.SearchFromPC(QuestionUtility.GetRandomQuestion());
                };

                PlayEndAnimation();
            }
        }

        private async void PlayStartAnimation()
        {
            await mainBorder.SlideIn(1000);
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
            pb.Visibility = Visibility.Visible;
            MicrosoftAccount microsoftAccount = new(emailText.Text.Trim(), pwdText.Password.Trim());
            await microsoftAccount.Login();
            RewardAccount rewardAccount = new();
            rewardAccount.AddCookies(microsoftAccount.GetCookies());
            if (await rewardAccount.Login())
            {
                string? userInfo = await rewardAccount.GetuserInfo();
                if (userInfo != null)
                {
                    rewardAccount.SaveCookies(COOKIE_PATH);
                }
            }
            else
            {
                PlayErrorAnimation();
                mainBorder.IsEnabled = true;
            }
            pb.Visibility = Visibility.Collapsed;
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
