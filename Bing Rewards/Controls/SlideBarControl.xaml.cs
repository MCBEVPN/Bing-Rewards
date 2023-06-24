using Bing_Rewards.Account;
using General.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bing_Rewards.Controls
{
    /// <summary>
    /// SlideBar.xaml 的交互逻辑
    /// </summary>
    public partial class SlideBarControl : UserControl
    {
        public const string PATH = "./accounts.json";
        public static List<RewardAccount> RewardAccounts { get; set; } = new();

        public event EventHandler? PageActive;


        public SlideBarControl()
        {
            InitializeComponent();
            Initilize();
        }

        private void Initilize()
        {
            if (File.Exists(PATH))
            {
                Span<RewardAccount> rewardAccounts = RewardAccount.LoadAccounts(PATH);
                RewardAccounts = new();
                for (int i = 0; i < rewardAccounts.Length; i++)
                {
                    RewardAccounts.Add(rewardAccounts[i]);
                    TabControl tabControl = new()
                    {
                        Margin = new Thickness(0, 0, 0, 5),
                        Account = rewardAccounts[i],
                        
                    };
                    tabControl.Refresh();
                    tabControl.LoginActive += (s, e) =>
                    {
                        PageActive?.Invoke(tabControl, e);
                    };
                    tabControl.Delete += (s, e) =>
                    {
                        stack.Children.Remove(tabControl);
                    };
                    stack.Children.Insert(stack.Children.Count - 1, tabControl);
                }
            }
        }

        public static void SaveAccounts()
        {
            RewardAccount.SaveAccounts(PATH, RewardAccounts.ToArray());
        }

        private void AddAccount_Click(object sender, RoutedEventArgs e)
        {
            TabControl tabControl = new()
            {
                Margin = new Thickness(0, 0, 0, 5)
            };
            tabControl.LoginActive += (s, e) =>
            {
                PageActive?.Invoke(tabControl, e);
            };
            tabControl.Delete += (s, e) =>
            {
                stack.Children.Remove(tabControl);
            };
            stack.Children.Insert(stack.Children.Count - 1, tabControl);
            tabControl.SetActive();
        }
    }
}
