using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tricorder.Mobile.Converters
{
    public class IsNotNullConverter : IValueConverter
    {
        public static IsNotNullConverter Instance { get; } = new IsNotNullConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(value is null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
