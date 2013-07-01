using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// A two-dimensional point.
    /// </summary>
    public struct PointS : IEquatable<PointS>
    {
        /// <summary>
        /// Gets the X component of the point.
        /// </summary>
        public short X { get; private set; }

        /// <summary>
        /// Gets the Y component of the point.
        /// </summary>
        public short Y { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointS"/> struct.
        /// </summary>
        /// <param name="x">The value to initialize the X component with.</param>
        /// <param name="y">The value to initialize the Y component with.</param>
        public PointS(short x, short y)
            : this()
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets the maximum components between two points.
        /// </summary>
        /// <returns>a new <see cref="PointS"/> which has the maximized components.</returns>
        public static PointS MaxComponents(PointS a, PointS b)
        {
            var x = Math.Max(a.X, b.X);
            var y = Math.Max(a.Y, b.Y);
            return new PointS(x, y);
        }

        /// <summary>
        /// Gets the minimum components between two points.
        /// </summary>
        /// <returns>a new <see cref="PointS"/> which has the minimized components.</returns>
        public static PointS MinComponents(PointS a, PointS b)
        {
            var x = Math.Min(a.X, b.X);
            var y = Math.Min(a.Y, b.Y);
            return new PointS(x, y);
        }

        /// <inheritdoc cref="Add(PointS, PointS)" />
        public static PointS operator +(PointS a, PointS b)
        {
            return Add(a, b);
        }

        /// <summary>
        /// Sums of the components of two points.
        /// </summary>
        /// <returns>a <see cref="PointS"/> with the summed components.</returns>
        public static PointS Add(PointS a, PointS b)
        {
            return new PointS((short)(a.X + b.X), (short)(a.Y + b.Y));
        }

        /// <inheritdoc cref="Negate(PointS)" />
        public static PointS operator -(PointS a)
        {
            return Negate(a);
        }

        /// <summary>
        /// Negates a point.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <returns>a <see cref="PointS"/> with negated components.</returns>
        public static PointS Negate(PointS a)
        {
            if (a.X == short.MinValue || a.Y == short.MinValue)
            {
                throw new ArgumentException(Exceptions.PointComponentsMustBeLargerThanMinValue, "a");
            }

            return new PointS((short)(-a.X), (short)(-a.Y));
        }

        /// <summary>
        /// Checks two <see cref="PointS"/> objects for value equality.
        /// </summary>
        public static bool operator ==(PointS a, PointS b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Checks two <see cref="PointS"/> objects for value inequality.
        /// </summary>
        public static bool operator !=(PointS a, PointS b)
        {
            return !(a == b);
        }

        #region Implementation of IEquatable<PointS>

        /// <inheritdoc />
        public bool Equals(PointS other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        #endregion

        #region Equality members

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is PointS && this.Equals((PointS)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
            }
        }

        #endregion
    }
}