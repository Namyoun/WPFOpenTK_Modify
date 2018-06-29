#region --- License ---
/*
Copyright (c) 2006 - 2008 The Open Toolkit library.

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */
#endregion

using System;
using System.Runtime.InteropServices;

namespace OpenTK.Math
{
    /// <summary>Represents a 2D vector using two integers.</summary>
    /// <remarks>
    /// The Vector2i structure is suitable for interoperation with unmanaged code requiring two consecutive ints.
    /// </remarks>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2i : IEquatable<Vector2i>
    {
        #region Fields

        /// <summary>
        /// The X component of the Vector2i.
        /// </summary>
        public int X;

        /// <summary>
        /// The Y component of the Vector2i.
        /// </summary>
        public int Y;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new Vector2i.
        /// </summary>
        /// <param name="x">The x coordinate of the net Vector2i.</param>
        /// <param name="y">The y coordinate of the net Vector2i.</param>
        public Vector2i(int x, int y)
        {
			X = x;
			Y = y;
        }

        /// <summary>
        /// Constructs a new Vector2i from the given Vector2i.
        /// </summary>
        /// <param name="v">The Vector2i to copy components from.</param>
        [Obsolete]
        public Vector2i(Vector2i v)
        {
            X = v.X;
            Y = v.Y;
        }

        /// <summary>
        /// Constructs a new Vector2i from the given Vector3.
        /// </summary>
        /// <param name="v">The Vector3 to copy components from. Z is discarded.</param>
        [Obsolete]
        public Vector2i(Vector3 v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
        }

        /// <summary>
        /// Constructs a new Vector2i from the given Vector4.
        /// </summary>
        /// <param name="v">The Vector4 to copy components from. Z and W are discarded.</param>
        [Obsolete]
        public Vector2i(Vector4 v)
        {
            X = (int)v.X;
            Y = (int)v.Y;
        }

        #endregion

        #region Public Members

        #region Instance

        #region public void Add()

        /// <summary>Add the Vector passed as parameter to this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        public void Add( Vector2i right )
        {
            this.X += right.X;
            this.Y += right.Y;
        }

        /// <summary>Add the Vector passed as parameter to this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        [CLSCompliant(false)]
        public void Add( ref Vector2i right )
        {
            this.X += right.X;
            this.Y += right.Y;
        }

        #endregion public void Add()

        #region public void Sub()

        /// <summary>Subtract the Vector passed as parameter from this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        public void Sub( Vector2i right )
        {
            this.X -= right.X;
            this.Y -= right.Y;
        }

        /// <summary>Subtract the Vector passed as parameter from this instance.</summary>
        /// <param name="right">Right operand. This parameter is only read from.</param>
        [CLSCompliant(false)]
        public void Sub( ref Vector2i right )
        {
            this.X -= right.X;
            this.Y -= right.Y;
        }

        #endregion public void Sub()

        #region public void Mult()

        /// <summary>Multiply this instance by a scalar.</summary>
        /// <param name="i">Scalar operand.</param>
        public void Mult( int i )
        {
            this.X *= i;
            this.Y *= i;
        }

        #endregion public void Mult()

        #region public void Div()

        /// <summary>Divide this instance by a scalar.</summary>
        /// <param name="i">Scalar operand.</param>
        public void Div( int i )
        {
            this.X /= i;
            this.Y /= i;
        }

        #endregion public void Div()

        #region public float Length

        /// <summary>
        /// Gets the length (magnitude) of the vector.
        /// </summary>
        /// <see cref="LengthFast"/>
        /// <seealso cref="LengthSquared"/>
        public float Length
        {
			get
			{
				return (float)System.Math.Sqrt(X * X + Y * Y);
			}
        }

        #endregion

        #region public int LengthFast

        /// <summary>
        /// Gets an approximation of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property uses an approximation of the square root function to calculate vector magnitude, with
        /// an upper error bound of 0.001.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthSquared"/>
        public float LengthFast
        {
			get
			{
				return 1.0f / OpenTK.Math.Functions.InverseSqrtFast(X * X + Y * Y);
			}
        }

        #endregion

        #region public int LengthSquared

        /// <summary>
        /// Gets the square of the vector length (magnitude).
        /// </summary>
        /// <remarks>
        /// This property avoids the costly square root operation required by the Length property. This makes it more suitable
        /// for comparisons.
        /// </remarks>
        /// <see cref="Length"/>
        /// <seealso cref="LengthFast"/>
        public float LengthSquared
        {
			get
			{
				return X * X + Y * Y;
			}
        }

		#endregion

		#region public Vector2i PerpendicularRight

		/// <summary>
		/// Gets the perpendicular vector on the right side of this vector.
		/// </summary>
		public Vector2i PerpendicularRight
		{
			get
			{
				return new Vector2i(Y, -X);
			}
		}

		#endregion

		#region public Vector2i PerpendicularLeft

		/// <summary>
		/// Gets the perpendicular vector on the left side of this vector.
		/// </summary>
		public Vector2i PerpendicularLeft
		{
			get
			{
				return new Vector2i(-Y, X);
			}
		}

		#endregion

        #region public void Normalize()

        /// <summary>
        /// Scales the Vector2i to unit length.
        /// </summary>
        public void Normalize()
        {
            //float length = this.Length;
            //X /= length;
            //Y /= length;
        }

        #endregion

        #region public void NormalizeFast()

        /// <summary>
        /// Scales the Vector2i to approximately unit length.
        /// </summary>
        public void NormalizeFast()
        {
            //float scale = Functions.InverseSqrtFast(X * X + Y * Y);
            //X *= scale;
            //Y *= scale;
        }

        #endregion

        #region public void Scale()

        /// <summary>
        /// Scales the current Vector2i by the given amounts.
        /// </summary>
        /// <param name="sx">The scale of the X component.</param>
        /// <param name="sy">The scale of the Y component.</param>
        public void Scale(int sx, int sy)
        {
			this.X = X * sx;
			this.Y = Y * sy;
        }

        /// <summary>Scales this instance by the given parameter.</summary>
        /// <param name="scale">The scaling of the individual components.</param>
        public void Scale( Vector2i scale )
        {
            this.X *= scale.X;
            this.Y *= scale.Y;
        }

        /// <summary>Scales this instance by the given parameter.</summary>
        /// <param name="scale">The scaling of the individual components.</param>
        [CLSCompliant(false)]
        public void Scale( ref Vector2i scale )
        {
            this.X *= scale.X;
            this.Y *= scale.Y;
        }

        #endregion public void Scale()

        #endregion

        #region Static

        #region Fields

        /// <summary>
        /// Defines a unit-length Vector2i that points towards the X-axis.
        /// </summary>
        public static readonly Vector2i UnitX = new Vector2i(1, 0);

        /// <summary>
        /// Defines a unit-length Vector2i that points towards the Y-axis.
        /// </summary>
        public static readonly Vector2i UnitY = new Vector2i(0, 1);

        /// <summary>
        /// Defines a zero-length Vector2i.
        /// </summary>
        public static readonly Vector2i Zero = new Vector2i(0, 0);

        /// <summary>
        /// Defines an instance with all components set to 1.
        /// </summary>
        public static readonly Vector2i One = new Vector2i(1, 1);
        
        /// <summary>
        /// Defines the size of the Vector2i struct in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf(new Vector2i());

        #endregion

        #region Add

        /// <summary>
        /// Add two Vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of addition</returns>
        public static Vector2i Add(Vector2i a, Vector2i b)
        {
            a.X += b.X;
            a.Y += b.Y;
            return a;
        }

        /// <summary>
        /// Add two Vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of addition</param>
        public static void Add(ref Vector2i a, ref Vector2i b, out Vector2i result)
        {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
        }

        #endregion

        #region Sub

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>Result of subtraction</returns>
        public static Vector2i Sub(Vector2i a, Vector2i b)
        {
            a.X -= b.X;
            a.Y -= b.Y;
            return a;
        }

        /// <summary>
        /// Subtract one Vector from another
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">Result of subtraction</param>
        public static void Sub(ref Vector2i a, ref Vector2i b, out Vector2i result)
        {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
        }

        #endregion

        #region Mult

        /// <summary>
        /// Multiply a vector and a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="i">Scalar operand</param>
        /// <returns>Result of the multiplication</returns>
        public static Vector2i Mult(Vector2i a, int i)
        {
            a.X *= i;
            a.Y *= i;
            return a;
        }

        /// <summary>
        /// Multiply a vector and a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="i">Scalar operand</param>
        /// <param name="result">Result of the multiplication</param>
        public static void Mult(ref Vector2i a, int i, out Vector2i result)
        {
            result.X = a.X * i;
            result.Y = a.Y * i;
        }

        #endregion

        #region Div

        /// <summary>
        /// Divide a vector by a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="i">Scalar operand</param>
        /// <returns>Result of the division</returns>
        public static Vector2i Div(Vector2i a, int i)
        {
            a.X /= i;
            a.Y /= i;
            return a;
        }

        /// <summary>
        /// Divide a vector by a scalar
        /// </summary>
        /// <param name="a">Vector operand</param>
        /// <param name="i">Scalar operand</param>
        /// <param name="result">Result of the division</param>
        public static void Div(ref Vector2i a, int i, out Vector2i result)
        {
            result.X = a.X / i;
            result.Y = a.Y / i;
        }

        #endregion

        #region ComponentMin

        /// <summary>
        /// Calculate the component-wise minimum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise minimum</returns>
        public static Vector2i ComponentMin(Vector2i a, Vector2i b)
        {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Calculate the component-wise minimum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise minimum</param>
        public static void ComponentMin(ref Vector2i a, ref Vector2i b, out Vector2i result)
        {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
        }

        #endregion

        #region ComponentMax

        /// <summary>
        /// Calculate the component-wise maximum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <returns>The component-wise maximum</returns>
        public static Vector2i ComponentMax(Vector2i a, Vector2i b)
        {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            return a;
        }

        /// <summary>
        /// Calculate the component-wise maximum of two vectors
        /// </summary>
        /// <param name="a">First operand</param>
        /// <param name="b">Second operand</param>
        /// <param name="result">The component-wise maximum</param>
        public static void ComponentMax(ref Vector2i a, ref Vector2i b, out Vector2i result)
        {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
        }

        #endregion

        #region Min

        /// <summary>
        /// Returns the Vector3 with the minimum magnitude
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        public static Vector2i Min(Vector2i left, Vector2i right)
        {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        #endregion

        #region Max

        /// <summary>
        /// Returns the Vector3 with the minimum magnitude
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="right">Right operand</param>
        /// <returns>The minimum Vector3</returns>
        public static Vector2i Max(Vector2i left, Vector2i right)
        {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        #endregion

        #region Clamp

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors
        /// </summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <returns>The clamped vector</returns>
        public static Vector2i Clamp(Vector2i vec, Vector2i min, Vector2i max)
        {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            return vec;
        }

        /// <summary>
        /// Clamp a vector to the given minimum and maximum vectors
        /// </summary>
        /// <param name="vec">Input vector</param>
        /// <param name="min">Minimum vector</param>
        /// <param name="max">Maximum vector</param>
        /// <param name="result">The clamped vector</param>
        public static void Clamp(ref Vector2i vec, ref Vector2i min, ref Vector2i max, out Vector2i result)
        {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Scale a vector to unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector2 Normalize(Vector2i vec)
        {
            var result = new Vector2(vec.X, vec.Y);
            float scale = 1.0f / result.Length;
            result.X *= scale;
            result.Y *= scale;
            return result;
        }

        /// <summary>
        /// Scale a vector to unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void Normalize(ref Vector2i vec, out Vector2 result)
        {
            result.X = vec.X;
            result.Y = vec.Y;
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
        }

        #endregion

        #region NormalizeFast

        /// <summary>
        /// Scale a vector to approximately unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <returns>The normalized vector</returns>
        public static Vector2 NormalizeFast(Vector2i vec)
        {
            var result = new Vector2(vec.X, vec.Y);
            float scale = Functions.InverseSqrtFast(result.X * result.X + result.Y * result.Y);
            result.X *= scale;
            result.Y *= scale;
            return result;
        }

        /// <summary>
        /// Scale a vector to approximately unit length
        /// </summary>
        /// <param name="vec">The input vector</param>
        /// <param name="result">The normalized vector</param>
        public static void NormalizeFast(ref Vector2i vec, out Vector2 result)
        {
            result.X = vec.X;
            result.Y = vec.Y;
            float scale = Functions.InverseSqrtFast(result.X * result.X + result.Y * result.Y);
            result.X *= scale;
            result.Y *= scale;
        }

        #endregion

        #region Dot

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <returns>The dot product of the two inputs</returns>
        public static int Dot(Vector2i left, Vector2i right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        /// <summary>
        /// Calculate the dot (scalar) product of two vectors
        /// </summary>
        /// <param name="left">First operand</param>
        /// <param name="right">Second operand</param>
        /// <param name="result">The dot product of the two inputs</param>
        public static void Dot( ref Vector2i left, ref Vector2i right, out int result )
        {
            result = left.X * right.X + left.Y * right.Y;
        }

        #endregion

        #region Lerp

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <returns>a when blend=0, b when blend=1, and a linear combination otherwise</returns>
        public static Vector2i Lerp(Vector2i a, Vector2i b, int blend)
        {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            return a;
        }

        /// <summary>
        /// Returns a new Vector that is the linear blend of the 2 given Vectors
        /// </summary>
        /// <param name="a">First input vector</param>
        /// <param name="b">Second input vector</param>
        /// <param name="blend">The blend factor. a when blend=0, b when blend=1.</param>
        /// <param name="result">a when blend=0, b when blend=1, and a linear combination otherwise</param>
        public static void Lerp( ref Vector2i a, ref Vector2i b, int blend, out Vector2i result )
        {
            result.X = blend * ( b.X - a.X ) + a.X;
            result.Y = blend * ( b.Y - a.Y ) + a.Y;
        }

        #endregion

        #region Barycentric

        /// <summary>
        /// Interpolate 3 Vectors using Barycentric coordinates
        /// </summary>
        /// <param name="a">First input Vector</param>
        /// <param name="b">Second input Vector</param>
        /// <param name="c">Third input Vector</param>
        /// <param name="u">First Barycentric Coordinate</param>
        /// <param name="v">Second Barycentric Coordinate</param>
        /// <returns>a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</returns>
        public static Vector2i BaryCentric(Vector2i a, Vector2i b, Vector2i c, int u, int v)
        {
            return a + u * (b - a) + v * (c - a);
        }

        /// <summary>Interpolate 3 Vectors using Barycentric coordinates</summary>
        /// <param name="a">First input Vector.</param>
        /// <param name="b">Second input Vector.</param>
        /// <param name="c">Third input Vector.</param>
        /// <param name="u">First Barycentric Coordinate.</param>
        /// <param name="v">Second Barycentric Coordinate.</param>
        /// <param name="result">Output Vector. a when u=v=0, b when u=1,v=0, c when u=0,v=1, and a linear combination of a,b,c otherwise</param>
        public static void BaryCentric( ref Vector2i a, ref Vector2i b, ref Vector2i c, int u, int v, out Vector2i result )
        {
            result = a; // copy

            Vector2i temp = b; // copy
            temp.Sub( ref a );
            temp.Mult( u );
            result.Add( ref temp );

            temp = c; // copy
            temp.Sub( ref a );
            temp.Mult( v );
            result.Add( ref temp );
        }

        #endregion

        #endregion

        #region Operators

		public static Vector2i operator +(Vector2i left, Vector2i right)
		{
			left.X += right.X;
			left.Y += right.Y;
			return left;
		}

		public static Vector2i operator -(Vector2i left, Vector2i right)
		{
			left.X -= right.X;
			left.Y -= right.Y;
			return left;
		}

		public static Vector2i operator -(Vector2i vec)
		{
			vec.X = -vec.X;
			vec.Y = -vec.Y;
			return vec;
		}

		public static Vector2i operator *(Vector2i vec, int i)
		{
			vec.X *= i;
			vec.Y *= i;
			return vec;
		}

		public static Vector2i operator *(int i, Vector2i vec)
		{
			vec.X *= i;
			vec.Y *= i;
			return vec;
		}

		public static Vector2i operator /(Vector2i vec, int i)
		{
            vec.X /= i;
            vec.Y /= i;
			return vec;
		}

        public static bool operator ==(Vector2i left, Vector2i right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2i left, Vector2i right)
        {
            return !left.Equals(right);
        }

        #endregion

        #region Overrides

        #region public override string ToString()

        /// <summary>
        /// Returns a System.String that represents the current Vector2i.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }

        #endregion

        #region public override int GetHashCode()

        /// <summary>
        /// Returns the hashcode for this instance.
        /// </summary>
        /// <returns>A System.Int32 containing the unique hashcode for this instance.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        #endregion

        #region public override bool Equals(object obj)

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>True if the instances are equal; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2i))
                return false;

            return this.Equals((Vector2i)obj);
        }

        #endregion

        #endregion

        #endregion

        #region IEquatable<Vector2i> Members

        /// <summary>Indicates whether the current vector is equal to another vector.</summary>
        /// <param name="other">A vector to compare with this vector.</param>
        /// <returns>true if the current vector is equal to the vector parameter; otherwise, false.</returns>
        public bool Equals(Vector2i other)
        {
            return
                X == other.X &&
                Y == other.Y;
        }

        #endregion
    }
}
