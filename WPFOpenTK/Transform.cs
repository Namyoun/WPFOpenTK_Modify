using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics;
using OpenTK.Math;
using System.Windows; // for DP
using System.Windows.Media.Media3D;

namespace OpenGL
{
    /// <summary> This is just a simple transformation class with easily paramaterizable properties. </summary>
    public sealed class TransformGL : DependencyObject
    {
        public Vector3 Location = new Vector3();
        public Vector3 Rotation = new Vector3();
        public Vector3 Scale = new Vector3(1,1,1);

        #region RotationDP
        /// <summary> The roation (in degrees) around each axis </summary>
        public Point3D RotationDP
        {
            get { return (Point3D)GetValue(RotationDPProperty); }
            set { SetValue(RotationDPProperty, value); }
        }
        // Using a DependencyProperty as the backing store for RotationDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationDPProperty =
            DependencyProperty.Register("RotationDP", typeof(Point3D), typeof(TransformGL), new UIPropertyMetadata(new Point3D(), new PropertyChangedCallback(Rotation_Changed)));
        static void Rotation_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var transform = sender as TransformGL;

            var input = (Point3D)e.NewValue;
            var newVal = (Point3D)e.OldValue;
            if (input.X != -1000)
                newVal.X = input.X;
            if (input.Y != -1000)
                newVal.Y = input.Y;
            if (input.Z != -1000)
                newVal.Z = input.Z;
            transform.RotationDP = newVal;

            transform.Rotation.X = (float)newVal.X;
            transform.Rotation.Y = (float)newVal.Y;
            transform.Rotation.Z = (float)newVal.Z;
        }
        #endregion

        #region LocationDP
        public Point3D LocationDP
        {
            get { return (Point3D)GetValue(LocationDPProperty); }
            set { SetValue(LocationDPProperty, value); }
        }
        // Using a DependencyProperty as the backing store for LocationDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationDPProperty =
            DependencyProperty.Register("LocationDP", typeof(Point3D), typeof(TransformGL), new UIPropertyMetadata(new Point3D(), new PropertyChangedCallback(Location_Changed)));
        static void Location_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var transform = sender as TransformGL;
            var input = (Point3D)e.NewValue;
            var newVal = (Point3D)e.OldValue;
            if (input.X != -1000)
                newVal.X = input.X;
            if (input.Y != -1000)
                newVal.Y = input.Y;
            if (input.Z != -1000)
                newVal.Z = input.Z;
            transform.LocationDP = newVal;
            transform.Location.X = (float)newVal.X;
            transform.Location.Y = (float)newVal.Y;
            transform.Location.Z = (float)newVal.Z;
        }
        #endregion

        #region ScaleDP
        public Point3D ScaleDP
        {
            get { return (Point3D)GetValue(ScaleDPProperty); }
            set { SetValue(ScaleDPProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ScaleDP.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleDPProperty =
            DependencyProperty.Register("ScaleDP", typeof(Point3D), typeof(TransformGL), new UIPropertyMetadata(new Point3D(), new PropertyChangedCallback(Scale_Changed)));
        static void Scale_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var transform = sender as TransformGL;
            var input = (Point3D)e.NewValue;
            var newVal = (Point3D)e.OldValue;
            if (input.X != -1000)
                newVal.X = input.X;
            if (input.Y != -1000)
                newVal.Y = input.Y;
            if (input.Z != -1000)
                newVal.Z = input.Z;
            transform.ScaleDP = newVal;
            transform.Scale.X = (float)newVal.X;
            transform.Scale.Y = (float)newVal.Y;
            transform.Scale.Z = (float)newVal.Z;
        }
        #endregion

        #region Constructor
        public TransformGL() { }
        public TransformGL(Vector3 location)
        {
            this.Location = location;
        }
        public TransformGL(Vector3 location, Vector3 rotation)
        {
            this.Location = location;
            this.Rotation = rotation;
        }
        public TransformGL(Vector3 location, Vector3 rotation, Vector3 scale)
        {
            this.Location = location;
            this.Rotation = rotation;
            this.Scale = scale;
        } 
        #endregion

        public void Apply()
        {
            GL.PushMatrix();
            GL.Translate(Location);
            GL.Rotate(Rotation.Z, Vector3.UnitZ);
            GL.Rotate(Rotation.Y, Vector3.UnitY);
            GL.Rotate(Rotation.X, Vector3.UnitX);
            GL.Scale(Scale);
        }

        public void Remove()
        {
            GL.PopMatrix();
        }

        /// <summary> Synchronize the fields with the dependency properties. </summary>
        /// <remarks> Only needed during animations </remarks>
        public void Update()
        {
            LocationDP.ToVector3(out Location);
            RotationDP.ToVector3(out Rotation);
        }
    }
}