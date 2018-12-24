using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Tricorder.Mobile.Converters
{
    public class GuidToStringConverter : IValueConverter
    {
        public static GuidToStringConverter Instance { get; } = new GuidToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Guid guid)
            {
                return guid.ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                Guid guid;

                if (Guid.TryParse(str, out guid))
                {
                    return guid;
                }
            }

            return value;
        }
    }
}
