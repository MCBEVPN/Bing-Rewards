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
        public static async void FadeIn(this UIElement element, int duration)
        {
            await FadeIn(element, duration, 0, 1);
        }
        public static async Task FadeInAsync(this UIElement element, int duration)
        {
            await FadeIn(element, duration, 0, 1);
        }

        public static async void FadeOut(this UIElement element, int duration)
        {
            await FadeOut(element, duration, 1, 0);
        }

        public static async Task FadeOutAsync(this UIElement element, int duration)
        {
            await FadeOut(element, duration, 1, 0);
        }

        public static async Task FadeIn(this UIElement element, int duration,double from ,double to)
        {
            DoubleAnimation animation = new(from, to, new Duration(TimeSpan.FromMilliseconds(duration)));
            element.BeginAnimation(UIElement.OpacityProperty, animation);
            await Task.Delay(duration);
        }

        public static async Task FadeOut(this UIElement element, int duration, double from, double to)
        {
            DoubleAnimation animation = new(from, to, new Duration(TimeSpan.FromMilliseconds(duration)));
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

            element.FadeIn(duration);
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

            element.FadeOut(duration);
            await Task.Delay(duration);
        }

        public static async Task SlideBack(this UIElement element, int duration, double from, double to)
        {
            element.RenderTransform = new TranslateTransform(from, 0);
            DoubleAnimation animation = new(from, to, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.XProperty, animation);

            element.FadeIn(duration);
            await Task.Delay(duration);
        }

        public static async Task SlideIn(this UIElement element, int duration)
        {
            await SlideIn(element, duration, 256, 0);
        }

        public static async Task SlideOut(this UIElement element, int duration)
        {
            await SlideOut(element, duration, 0, -256);
        }

        public static async Task SlideBack(this UIElement element, int duration)
        {
            await SlideBack(element, duration, -256, 0);
        }

        public static async Task SlideInFromTop(this UIElement element, int duration)
        {
            await SlideInFromTop(element, duration, -256, 0);
        }

        public static Task SlideInFromTop(this UIElement element, int duration, int v1, int v2)
        {
            element.RenderTransform = new TranslateTransform(0, v1);
            DoubleAnimation animation = new(v1, v2, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);

            element.FadeIn(duration);
            return Task.Delay(duration);
        }

        public static Task SlideOutFromTop(this UIElement element, int duration, int v1, int v2)
        {
            element.RenderTransform = new TranslateTransform(0, v1);
            DoubleAnimation animation = new(v1, v2, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);

            element.FadeOut(duration);
            return Task.Delay(duration);
        }

        public static async Task SlideInFromBottom(this UIElement element, int duration)
        {
            await SlideInFromBottom(element, duration, 256, 0);
        }

        public static async Task SlideOutFromTop(this UIElement element, int duration)
        {
            await SlideOutFromTop(element, duration, 0, -256);
        }

        public static async Task SlideOutFromBottom(this UIElement element, int duration)
        {
            await SlideOutFromBottom(element, duration, 0, 256);
        }

        public static Task SlideInFromBottom(this UIElement element, int duration, int v1, int v2)
        {
            element.RenderTransform = new TranslateTransform(0, v1);
            DoubleAnimation animation = new(v1, v2, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);

            element.FadeIn(duration);
            return Task.Delay(duration);
        }

        public static Task SlideOutFromBottom(this UIElement element, int duration, int v1, int v2)
        {
            element.RenderTransform = new TranslateTransform(0, v1);
            DoubleAnimation animation = new(v1, v2, new Duration(TimeSpan.FromMilliseconds(duration)))
            {
                EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
            };
            element.RenderTransform.BeginAnimation(TranslateTransform.YProperty, animation);

            element.FadeOut(duration);
            return Task.Delay(duration);
        }
    }
}
