using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tricorder.Mobile.Converters
{
    public class ByteArrayToStringConverter : IValueConverter
    {
        public static ByteArrayToStringConverter Instance { get; } = new ByteArrayToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] ba)
            {
                string str = System.Text.Encoding.Default.GetString(ba);
                return str;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
