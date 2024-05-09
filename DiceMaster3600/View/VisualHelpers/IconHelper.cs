using System.Windows;

namespace DiceMaster3600.View.VisualHelpers
{
    public static class IconProperties
    {
        public static readonly DependencyProperty IconKindProperty =
            DependencyProperty.RegisterAttached(
                "IconKind",
                typeof(string),
                typeof(IconProperties),
                new PropertyMetadata(default(string)));

        public static void SetIconKind(UIElement element, string value)
        {
            element.SetValue(IconKindProperty, value);
        }

        public static string GetIconKind(UIElement element)
        {
            return (string)element.GetValue(IconKindProperty);
        }
    }
}
