using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Windows;

namespace WPFOpenTK
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var b = (bool?)value;
            Visibility retVal;
            if (b.HasValue == true && b.Value == true)
                retVal = Visibility.Visible;
            else
                retVal = Visibility.Collapsed;
            return retVal;
        }

        // Should never be called
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
