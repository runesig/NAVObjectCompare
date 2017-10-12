using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace NAVObjectCompareWinClient.Helpers
{
    public class RowColorValue : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Brush[] brushes = parameter as Brush[];
            // var currentBrush = (SolidColorBrush)brushes[0];

            bool equal = false;
            if (bool.TryParse(value.ToString(), out equal))
            {
                if (equal)
                    return "White";
            }

            return "Salmon";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
