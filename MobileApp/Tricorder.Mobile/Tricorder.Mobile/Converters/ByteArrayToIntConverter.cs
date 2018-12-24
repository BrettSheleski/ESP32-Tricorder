using System;
using System.Globalization;
using Xamarin.Forms;

namespace Tricorder.Mobile.Converters
{
    public class ByteArrayToIntConverter : IValueConverter
    {
        public static ByteArrayToIntConverter Instance { get; } = new ByteArrayToIntConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] ba)
            {
                byte[] copy = new byte[ba.Length];

                ba.CopyTo(copy, 0);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(copy);

                int i = BitConverter.ToInt32(copy, 0);
                
                return i;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
