using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Data;
using System.Windows.Media.Media3D;
using OpenTK.Math;

namespace WPFOpenTK
{
    /// <summary>Convert a Point3D to a Vector3</summary>
    public class Vector3Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Vector3)
            {
                var vector3 = (Vector3)value;
                return new Point3D(vector3.X, vector3.Y, vector3.Z);
            }
            else if (value is string)
            {
                return new Vector3(10.0f, 10.0f, 10.0f);
            }
            return new Vector3(20.0f, 20.0f, 20.0f);
        }

        // Should never be called
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Point3D)
            {
                var point3D = (Point3D)value;
                return new Vector3((float)point3D.X, (float)point3D.Y, (float)point3D.Z);
            }
            else if (value is Vector3)
            {
                var vector3 = (Vector3)value;
                return new Point3D(vector3.X, vector3.Y, vector3.Z);
            }
            else if (value is string)
            {
                return new Vector3(10.0f, 10.0f, 10.0f);
            }
            return new Vector3(20.0f, 20.0f, 20.0f);
        }
    }
}
