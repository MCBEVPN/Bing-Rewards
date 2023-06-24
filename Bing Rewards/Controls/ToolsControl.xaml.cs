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
    /// ToolsControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsControl : UserControl
    {
        public event EventHandler? QueryClicked;
        public event EventHandler? HelpClicked;
        public event EventHandler? AboutClicked;

        public ToolsControl()
        {
            InitializeComponent();
        }

        private void QueryClick(object sender, RoutedEventArgs e)
        {
            QueryClicked?.Invoke(sender, e);
        }

        private void HelpClick(object sender, RoutedEventArgs e)
        {
            HelpClicked?.Invoke(sender, e);
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            AboutClicked?.Invoke(sender, e);
        }
    }
}
