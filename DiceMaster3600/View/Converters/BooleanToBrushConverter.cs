using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiceMaster3600.View.Converters
{
    public class BooleanToBrushConverter : IValueConverter
    {
        public Brush? TrueBrush { get; set; }
        public Brush? FalseBrush { get; set; }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool v && v ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as Brush == TrueBrush;
        }
    }
}
