using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Windows.Media.Media3D;
using OpenTK.Math;

namespace WPFOpenTK
{
    /// <summary>Set a breakpoint to debug bindings</summary>
    public class NumberConverter : IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null)
                parameter = 2;
            var format = "F" + parameter.ToString();
            format = "{0:" + format + "}";
            return String.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return double.Parse(value.ToString());
        }

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var digits = "2";
            if (values.Length > 1)
                digits = values[1].ToString();
            var format = "F" + digits;
            format = "{0:" + format + "}";
            return String.Format(format, values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return new object[] {double.Parse(value as string)};
        }

        #endregion
    }
}
