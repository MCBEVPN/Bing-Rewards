using Bing_Rewards.Account;
using Bing_Rewards.Controls;
using Bing_Rewards.Pages;
using Bing_Rewards.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UIShell.Controls;

namespace Bing_Rewards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private bool _IsDel;
        public MainWindow()
        {
            InitializeComponent();
            InitializeSlideBar();
            InitializeToolBar();
            DeactiveToolBar();
            frame.Navigated += Frame_Navigated;
        }

        private void InitializeSlideBar()
        {
            sbc.PageActive += (s, e) =>
            {
                if (s is Controls.TabControl tab)
                {
                    Page page = tab.GetPage();
                    if (frame.Content != page)
                    {
                        _ = frame.Navigate(page);
                    }
                    tab.Delete += (s, e) =>
                    {
                        _IsDel = true;
                        frame.Content = null;
                    };
                    tab.LoginSucceeded += (s, e) =>
                    {
                        _IsDel = false;
                        if (frame.Content == page)
                        {
                            page = tab.GetPage();
                            if (frame.Content != page)
                            {
                                _ = frame.Navigate(page);
                            }
                        }
                    };
                    if (!_IsDel)
                    {
                        _IsDel = false;
                    }
                }
            };
        }

        private void InitializeToolBar()
        {
            tc.QueryClicked += async (s, e) =>
            {
                if (frame.Content is SearcherPage searcher)
                {
                    QueryControl queryControl = new(searcher.Account)
                    {
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    _ = notic.Children.Add(queryControl);
                    await queryControl.SlideIn(500);
                    await queryControl.Initialize();
                    await Task.Delay(5000);
                    await queryControl.SlideOut(500, 0, 360);
                    notic.Children.Remove(queryControl);
                }
            };

            bool helpClicked = false;
            helpGrid.MouseDown += async (s, e) =>
            {
                if (!helpClicked)
                {
                    return;
                }
                helpClicked = false;
                helpGrid.FadeOut(500);
                await helpGrid.Children[0].SlideOutFromBottom(500);
                helpGrid.Visibility = Visibility.Collapsed;
                tc.IsEnabled = true;
                sbc.IsEnabled = true;
            };
            tc.HelpClicked += async (s, e) =>
            {
                helpClicked = true;
                tc.IsEnabled = false;
                sbc.IsEnabled = false;
                helpGrid.Visibility = Visibility.Visible;
                helpGrid.FadeIn(500);
                await helpGrid.Children[0].SlideInFromBottom(500);
            };

            bool aboutClicked = false;
            aboutGrid.MouseDown += async (s, e) =>
            {
                if (!aboutClicked)
                {
                    return;
                }
                aboutClicked = false;
                aboutGrid.FadeOut(500);
                await aboutGrid.Children[0].SlideOutFromBottom(500);
                aboutGrid.Visibility = Visibility.Collapsed;
                tc.IsEnabled = true;
                sbc.IsEnabled = true;
            };
            tc.AboutClicked += async (s, e) =>
            {
                aboutClicked = true;
                tc.IsEnabled = false;
                sbc.IsEnabled = false;
                aboutGrid.Visibility = Visibility.Visible;
                aboutGrid.FadeIn(500);
                await aboutGrid.Children[0].SlideInFromBottom(500);
            };
        }

        private async void ActiveToolBar()
        {
            if (tc.Visibility == Visibility.Visible)
            {
                await DeactiveToolBarAsync();
                tc.Visibility = Visibility.Visible;
                tc.IsEnabled = true;
                await tc.SlideInFromTop(500, -50, 0);
            }
            else
            {
                tc.Visibility = Visibility.Visible;
                tc.IsEnabled = true;
                await tc.SlideInFromTop(500, -50, 0);
            }
        }

        private async void DeactiveToolBar()
        {
            await tc.SlideOutFromTop(500, 0, -50);
            tc.IsEnabled = false;
            tc.Visibility = Visibility.Collapsed;
            _IsDel = false;
        }

        private async Task DeactiveToolBarAsync()
        {
            await tc.SlideOutFromTop(500, 0, -50);
            tc.IsEnabled = false;
            tc.Visibility = Visibility.Collapsed;
            _IsDel = false;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (_IsDel)
            {
                DeactiveToolBar();
            }
            else
            {
                ActiveToolBar();
            }
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await sbc.SlideBack(1000);
        }
    }
}
