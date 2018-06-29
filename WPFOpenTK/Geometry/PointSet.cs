using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenGL;
using OpenTK.Graphics;
using OpenTK.Math;

namespace OpenGL.Geometry
{
    /// <summary> A set of points to render. </summary>
    public abstract class PointSet : Geometry
    {
        protected const int DEFAULT_SIZE = 1000;
        protected List<Vector3> points = new List<Vector3>();
        protected Random rand = new Random();
        public float PointSize { get; set; }

        public PointSet() : this(DEFAULT_SIZE) { }

        public PointSet(int count)
        {
            Initialize(count);
        }
        protected PointSet(int count, bool inititialize)
        {
            if (inititialize)
                Initialize(count);
        }

        /// <summary> Generate the points </summary>
        /// <param name="count">The number of points</param>
        public abstract void Initialize(int count);

        /// <summary> Draw the points </summary>
        /// <remarks> Not very efficient. Use a <c>DisplayList</c> to render quickly </remarks>
        public override void Draw()
        {
            GL.PushAttrib(AttribMask.PointBit);
            GL.PointSize(PointSize);
            using (GLDraw.Begin(BeginMode.Points))
            {
                foreach (var point in points)
                {
                    GL.Vertex3(point);
                }
            }
            GL.PopAttrib();
        }

        public virtual void Optimize()
        {
            // to do: generate list and rerout Draw() to call list
            // need to properly dispose as well
        }
    }

    public class Cylinder : PointSet
    {
        public Cylinder() : base() { }
        public Cylinder(int count) : base(count) { }

        public override void Initialize(int count)
        {
            points = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                var theta = (float)rand.NextDouble() * 2 * OpenTK.Math.Functions.PIF;
                var y = (float)rand.NextDouble() * 2 - 1;
                points.Add(new Vector3((float)Math.Sin(theta), (float)Math.Cos(theta), y));
            }
        }
    }

    public class Sphere : PointSet
    {
        public Sphere() : this(DEFAULT_SIZE, 1) { }
        public Sphere(int count) : this(count, 1) { }
        public Sphere(int count, float radius) : base(count, false)
        {
            this.Radius = radius;
            Initialize(count);
        }

        public float Radius { get; set; }

        public override void Initialize(int count)
        {
            points = new List<Vector3>(count);
            for (int i = 0; i < count; i++)
            {
                var theta = (float)rand.NextDouble() * 2 * OpenTK.Math.Functions.PIF;
                var phi = (float)rand.NextDouble() * 2 - 1;
                phi = (float)Math.Asin(phi);
                var v = new Vector3();
                v.X = -(float)Math.Cos(theta) * (float)Math.Cos(phi);
                v.Y = (float)Math.Sin(phi);
                v.Z = (float)Math.Sin(theta) * (float)Math.Cos(phi);
                points.Add(v*Radius);
            }
        }
    }

    public class Box : PointSet
    {
        public Box() : base() { }
        public Box(int count) : base(count) { }

        public override void Initialize(int count)
        {
            points = new List<Vector3>(count);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < count/6; j++)
                {
                    var r1 = (float)rand.NextDouble() * 2 - 1;
                    var r2 = (float)rand.NextDouble() * 2 - 1;
                    switch (i)
                    {
                        case 0:
                            points.Add(new Vector3(r1, r2, -1));
                            break;
                        case 1:
                            points.Add(new Vector3(r1, r2, 1));
                            break;
                        case 2:
                            points.Add(new Vector3(r1, -1, r2));
                            break;
                        case 3:
                            points.Add(new Vector3(r1, 1, r2));
                            break;
                        case 4:
                            points.Add(new Vector3(-1, r1, r2));
                            break;
                        case 5:
                            points.Add(new Vector3(1, r1, r2));
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public override void Draw()
        {
            base.Draw();
        }
    }
}
