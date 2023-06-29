using Bing_Rewards.Account;
using General.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bing_Rewards.Controls
{
    /// <summary>
    /// QueryControl.xaml 的交互逻辑
    /// </summary>
    public partial class QueryControl : UserControl
    {
        public RewardAccount? Account { get; set; }
        public QueryControl(RewardAccount? rewardAccount)
        {
            Account = rewardAccount;
            InitializeComponent();
        }

        public QueryControl()
        {
            InitializeComponent();
        }

        public async Task Initialize()
        {
            if (Account == null)
            {
                SetText("不可用", "不可用", "不可用", "不可用");
            }
            else
            {
                string? json = await Account.GetuserInfo();
                if (json == null)
                {
                    SetText("不可用", "不可用", "不可用", "不可用");
                }
                else
                {
                    JObject stateObj = JObject.Parse(await Account.GetuserInfo());
                    if (stateObj["code"] == null)
                    {
                        string availablePoints = stateObj["status"]["userStatus"]["availablePoints"].ToString();
                        string lifetimePoints = stateObj["status"]["userStatus"]["lifetimePoints"].ToString();
                        string lifetimePointsRedeemed = stateObj["status"]["userStatus"]["lifetimePointsRedeemed"].ToString();
                        string lifetimeGivingPoints = stateObj["status"]["userStatus"]["lifetimeGivingPoints"].ToString();
                        SetText(availablePoints, lifetimePoints, lifetimePointsRedeemed, lifetimeGivingPoints);
                    }
                    else
                    {
                        SetText("已停用", "已停用", "已停用", "已停用");
                    }
                }
            }
        }

        private void SetText(string availablePoints, string lifetimePoints, string lifetimePointsRedeemed, string lifetimeGivingPoints)
        {
            this.availablePoints.Text = "可 用 积 分：" + availablePoints;
            this.lifetimePoints.Text = "积 分 总 计：" + lifetimePoints;
            this.lifetimePointsRedeemed.Text = "已使用积分：" + lifetimePointsRedeemed;
            this.lifetimeGivingPoints.Text = "已捐出积分：" + lifetimeGivingPoints;
        }
    }
}
