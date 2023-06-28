using Bing_Rewards.Account;
using Bing_Rewards.Dashboard;
using Bing_Rewards.States;
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

namespace Bing_Rewards.Pages
{
    /// <summary>
    /// SearcherPage.xaml 的交互逻辑
    /// </summary>
    public partial class SearcherPage : Page
    {
        public RewardAccount? Account { get; set; }
        public RunState RunState {
            get
            {
                return _RunState;
            }
            set
            {
                _RunState = value;
                switch (value)
                {
                    case RunState.Stopped:
                        start.Content = "开始";
                        pb.Visibility = Visibility.Collapsed;
                        break;
                    case RunState.Running:
                        start.Content = "停止";
                        pb.Visibility = Visibility.Visible;
                        StartSearch();
                        break;
                    case RunState.Stopping:
                        start.Content = "正在停止...";
                        break;
                        
                    case RunState.Completed:
                        start.Content = "完成";
                        break;
                }
            }
        }
        private RunState _RunState = RunState.Stopped;
        public SearcherPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            pb.Visibility = Visibility.Collapsed;
            this.Loaded += async (s, e) =>
            {
                await mainBorder.SlideInFromBottom(500);
            };
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            switch (RunState)
            {
                case RunState.Running:
                    RunState = RunState.Stopping;
                    break;
                case RunState.Stopped:
                    RunState = RunState.Running;
                    break;
                case RunState.Stopping:
                    RunState = RunState.Stopping;
                    break;
                case RunState.Completed:
                    RunState = RunState.Running;
                    break;
                default:
                    break;
            }
        }

        private async void StartSearch()
        {
            if (Account == null)
            {
                RunState = RunState.Stopped;
            }
            else
            {
                bool run = true;
                while (run)
                {
                    if (CheckStop())
                    {
                        break;
                    }
                    CompleteDashboard dashboard = await Account.GetComplete();
                    if (!dashboard.PC)
                    {
                        await Account.SearchFromPC(QuestionUtility.GetRandomQuestion());
                    }

                    if (CheckStop())
                    {
                        break;
                    }
                    if (!dashboard.Mobile)
                    {
                        await Account.SearchFromMobile(QuestionUtility.GetRandomQuestion());
                    }
                    run = !dashboard.PC || !dashboard.Mobile;
                }
            }
        }

        private bool CheckStop()
        {
            switch (RunState)
            {
                case RunState.Stopping:
                    RunState = RunState.Stopped;
                    return true;
                case RunState.Completed:
                    return true;
                default:
                    return false;
            }
        }
    }
}
