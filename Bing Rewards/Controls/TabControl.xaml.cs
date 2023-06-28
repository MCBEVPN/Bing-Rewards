using Bing_Rewards.Account;
using Bing_Rewards.Pages;
using Bing_Rewards.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bing_Rewards.Controls
{
    /// <summary>
    /// TabControl.xaml 的交互逻辑
    /// </summary>
    public partial class TabControl : UserControl
    {
        private static TabControl? _LastTabControl { get; set; }

        public event EventHandler? LoginSucceeded;
        public event EventHandler? LoginActive;
        public event EventHandler? Delete;

        public LoginPage LoginPage { get; set; } = new();
        public SearcherPage SearchPage { get; set; } = new();
        public RewardAccount? Account { get; set; }

        public TabControl()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            LoginPage.LoginSucceeded += (s, e) =>
            {
                Account = LoginPage.RewardAccount;
                if (Account != null)
                {
                    SlideBarControl.RewardAccounts.Add(Account);
                    SlideBarControl.SaveAccounts();
                    string? name = Account.UserName?.ToUpper();
                    if (name?.Length > 2)
                    {
                        title.Text = name[..2];
                    }
                    else
                    {
                        title.Text = name;
                    }
                }
                SearchPage.title.Text = Account?.UserName;
                LoginSucceeded?.Invoke(s, e);
            };
            MouseButtonState mouseButtonState = MouseButtonState.Released;
            this.MouseDown += (s, e) =>
            {
                mouseButtonState = e.MiddleButton;
            };
            this.MouseUp += (s, e) =>
            {
                switch (e.MiddleButton)
                {
                    case MouseButtonState.Released:
                        if (mouseButtonState == MouseButtonState.Pressed)
                        {
                            if (Account != null)
                            {
                                SlideBarControl.RewardAccounts.Remove(Account);
                                SlideBarControl.SaveAccounts();
                            }
                            Delete?.Invoke(this, new());
                        }
                        break;
                    default:
                        break;
                }
                mouseButtonState = e.MiddleButton;
            };
            this.MouseEnter += async (s, e) =>
            {
                await hoverBorder.FadeIn(300, 0, 0.5);
            };
            this.MouseLeave += async (s, e) =>
            {
                await hoverBorder.FadeOut(300, 0.5, 0);
            };
            this.MouseDoubleClick += (s, e) =>
            {
                if (Account != null)
                {
                    SlideBarControl.RewardAccounts.Remove(Account);
                    SlideBarControl.SaveAccounts();
                }
                Delete?.Invoke(this, new());
            };
            this.MouseLeftButtonDown += (s, e) =>
            {
                SetActive();
            };
        }

        public void Refresh()
        {
            if (Account != null)
            {
                SearchPage.Account = Account;
                if (Account.UserName is string name)
                {
                    this.ToolTip = name;
                    SearchPage.title.Text = name;
                    name = name.ToUpper();
                    if (name.Length > 2)
                    {
                        title.Text = name[..2];
                    }
                    else
                    {
                        title.Text = name;
                    }
                }
            }
        }

        public void SetActive()
        {
            LoginActive?.Invoke(this, new());
            if (_LastTabControl != null)
            {
                _LastTabControl.tabBorder.Background = new SolidColorBrush(Color.FromArgb(0, 108, 108, 108));
            }
            _LastTabControl = this;
            _LastTabControl.tabBorder.Background = new SolidColorBrush(Color.FromArgb(255 / 2, 108, 108, 108));
            Refresh();
        }

        public Page GetPage()
        {
            if (Account != null)
            {
                if (Account.IsLogin)
                {
                    Refresh();
                    return SearchPage;
                }
                else
                {
                    return LoginPage;
                }
            }
            else
            {
                return LoginPage;
            }
        }
    }
}
