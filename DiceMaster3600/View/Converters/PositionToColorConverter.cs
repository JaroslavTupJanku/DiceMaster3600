using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiceMaster3600.View.Converters
{
    public class PositionToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int position ? position switch
            {
                1 => new SolidColorBrush(Colors.Gold),
                2 => new SolidColorBrush(Colors.Silver),
                3 => new SolidColorBrush(Color.FromRgb(205, 127, 50)),
                _ => Brushes.Transparent,
            } : Brushes.Transparent;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
