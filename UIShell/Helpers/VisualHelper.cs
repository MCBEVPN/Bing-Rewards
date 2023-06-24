using System.Windows;
using System.Windows.Media;

namespace UIShell.Helpers
{
    internal static class VisualHelper
    {
        public static T? GetParent<T>(DependencyObject d) where T : DependencyObject
        {
            if (d is null)
            {
                return default;
            }

            return d is T t ? t : d is Window ? null : GetParent<T>(d is Visual ? VisualTreeHelper.GetParent(d) : LogicalTreeHelper.GetParent(d));
        }
    }
}