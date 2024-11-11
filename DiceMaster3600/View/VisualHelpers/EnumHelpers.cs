using System;
using System.ComponentModel;
using System.Linq;

namespace DiceMaster3600.View.VisualHelpers
{
    public static class EnumHelpers
    {
        public static string GetDescription(Enum enumValue)
        {
            var descriptionAttribute = enumValue.GetType()
                                                .GetField(enumValue.ToString())?
                                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                .FirstOrDefault() as DescriptionAttribute;

            return descriptionAttribute?.Description ?? enumValue.ToString();
        }
    }
}
