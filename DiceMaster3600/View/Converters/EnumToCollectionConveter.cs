using DiceMaster3600.Core.Enum;
using DiceMaster3600.View.VisualHelpers;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DiceMaster3600.View.Converters
{
    public class EnumToCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumType = typeof(UniversityType); // Zde je enum pevně nastaven
            return Enum.GetValues(enumType).Cast<Enum>()
                .Select(e => new { Value = e, Description = e.ToString() }).ToList(); // Změnil jsem na ToString() pro jednoduchost
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
