using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace OxfordChat.Client.Infrastructure
{
    class SentimentToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double?)) return new SolidColorBrush(Colors.DarkGray);
            var sentiment = 2*Math.Max(-1, Math.Min(1, (double)(value ?? 0.0))) - 1;

            return new SolidColorBrush(Color.FromRgb((byte)(127 - 100 * sentiment), (byte)(127 + 100 * sentiment), 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
