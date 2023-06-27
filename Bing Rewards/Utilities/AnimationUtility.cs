using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Bing_Rewards.Utilities
{
    public static class AnimationUtility
    {
        public static async Task FadeIn(this UIElement element, int duration)
        {
            DoubleAnimation animation = new(0, 1, new Duration(TimeSpan.FromMilliseconds(duration)));
            element.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay(duration);
        }

        public static async Task FadeOut(this UIElement element, int duration)
        {
            DoubleAnimation animation = new(1, 0, new Duration(TimeSpan.FromMilliseconds(duration)));
            element.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay(duration);
        }

        public static async Task SlideIn(this UIElement element, int duration, double from, double to)
        {
            element.RenderTransform = new TranslateTransform(from, 0);
            DoubleAnimation animation = new(from, to, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            DoubleAnimation animation2 = new(0, 1, new Duration(TimeSpan.FromMilliseconds(duration)));
            element.BeginAnimation(UIElement.OpacityProperty, animation2);

            await Task.Delay(duration);
        }

        public static async Task SlideOut(this UIElement element, int duration, double from, double to)
        {
            element.RenderTransform = new TranslateTransform(from, 0);
            DoubleAnimation animation = new(from, to, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            DoubleAnimation animation2 = new(1, 0, new Duration(TimeSpan.FromMilliseconds(duration)));
            element.BeginAnimation(UIElement.OpacityProperty, animation2);

            await Task.Delay(duration);
        }

        public static async Task SlideIn(this UIElement element, int duration)
        {
            await SlideIn(element, duration, 360, 0);
        }

        public static async Task SlideOut(this UIElement element, int duration)
        {
            await SlideOut(element, duration, 0, -360);
        }

        public static async Task SlideBack(this UIElement element, int duration)
        {
            await SlideOut(element, duration, -360, 0);
        }
    }
}
