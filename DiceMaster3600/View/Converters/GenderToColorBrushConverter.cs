using DiceMaster3600.Core.Enum;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DiceMaster3600.View.Converters
{
    public class GenderToColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) //Tady by ty brushes mohly byt jako verejne vlastnosti
        {
            return value.Equals(parameter) ? Brushes.Black : Brushes.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
