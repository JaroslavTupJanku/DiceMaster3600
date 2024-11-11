using DiceMaster3600.Core.Enum;
using DiceMaster3600.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DiceMaster3600.View.Converters
{
    public class GameNotificationTypeToColorConverter: IValueConverter
    {
        public Brush InformationBrush { get; set; }
        public Brush SuccessBrush { get; set; }
        public Brush ErrorBrush { get; set; }
        public Brush WarningBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                GameNotificationType.Information => InformationBrush,
                GameNotificationType.Success => SuccessBrush,
                GameNotificationType.Error => ErrorBrush,
                GameNotificationType.Warning => WarningBrush,
                _ => DependencyProperty.UnsetValue,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
