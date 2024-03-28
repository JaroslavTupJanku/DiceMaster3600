using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiceMaster3600.View.Helpers
{
    public static class TextBoxHelpers
    {
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.RegisterAttached(
            "PlaceholderText",
            typeof(string),
            typeof(TextBoxHelpers),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(TextBoxHelpers),
            new PropertyMetadata(new CornerRadius(0)));

        public static void SetPlaceholderText(UIElement element, string value) => element.SetValue(PlaceholderTextProperty, value);
        public static string GetPlaceholderText(UIElement element) => (string)element.GetValue(PlaceholderTextProperty);

        public static void SetCornerRadius(UIElement element, CornerRadius value) => element.SetValue(CornerRadiusProperty, value);
        public static CornerRadius GetCornerRadius(UIElement element) => (CornerRadius)element.GetValue(CornerRadiusProperty);

    }
}
