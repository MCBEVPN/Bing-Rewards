using Bing_Rewards.Account;
using Bing_Rewards.Pages;
using UIShell.Controls;

namespace Bing_Rewards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = frame.Navigate(new LoginPage());
        }
    }
}
