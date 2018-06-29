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
    public class Point3DConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Point3D)
            {
                var point3D = (Point3D)value;
                var str = parameter as string;
                if (parameter == null)
                    return value;
                double retVal = 0;
                if (str.ToUpper() == "X")
                    retVal = point3D.X;
                else if (str.ToUpper() == "Y")
                    retVal = point3D.Y;
                else if (str.ToUpper() == "Z")
                    retVal = point3D.Z;
                return retVal;
            }
            else if (value is Vector3)
            {
                var vector3 = (Vector3)value;
                return new Point3D(vector3.X, vector3.Y, vector3.Z);
            }
            else if (value is double)
            {
                var point3D = new Point3D((double)value, (double)value, (double)value);
                return point3D;

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
            if (value is double)
            {
                var point3D = new Point3D(-1000, -1000, -1000);
                var str = parameter as string;
                if (parameter == null)
                    return value;
                if (str.ToUpper() == "X")
                    point3D.X = (double)value;
                else if (str.ToUpper() == "Y")
                    point3D.Y = (double)value;
                else if (str.ToUpper() == "Z")
                    point3D.Z = (double)value;
                return point3D;
            }
            if (value is Point3D)
            {
                return value;
                //var point3D = (Point3D)value;
                //return new Vector3((float)point3D.X, (float)point3D.Y, (float)point3D.Z);
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

    public class Point3DMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is double)
                return values[0].ToString();

            var point3D = new Point3D(0,0,0);
            if (! (values[0] is double))
                return point3D; ;
            point3D.X = (double)values[0];
            point3D.Y = (double)values[1];
            point3D.Z = (double)values[2];
            return point3D;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
                return new object[] { double.Parse(value as string) };

            var point3D = (Point3D)value;
            var retVal = new object[] { point3D.X, point3D.Y, point3D.Z };
            return retVal;
        }
    }
}
