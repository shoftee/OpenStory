using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// A two-dimensional point.
    /// </summary>
    public struct PointS
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
        /// Initializes a new instance of <see cref="PointS"/>.
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
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
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
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        /// <returns>a new <see cref="PointS"/> which has the minimized components.</returns>
        public static PointS MinComponents(PointS a, PointS b)
        {
            var x = Math.Min(a.X, b.X);
            var y = Math.Min(a.Y, b.Y);
            return new PointS(x, y);
        }

        /// <summary>
        /// Sums of the components of two points.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <param name="b">A point.</param>
        /// <returns>a <see cref="PointS"/> with the summed components.</returns>
        public static PointS operator +(PointS a, PointS b)
        {
            return new PointS((short)(a.X + b.X), (short)(a.Y + b.Y));
        }

        /// <summary>
        /// Negates a point.
        /// </summary>
        /// <param name="a">A point.</param>
        /// <returns>a <see cref="PointS"/> with negated components.</returns>
        public static PointS operator -(PointS a)
        {
            if (a.X == short.MinValue || a.Y == short.MinValue)
            {
                throw new ArgumentOutOfRangeException("a", "The components of the provided point must be larger than Int16.MinValue.");
            }

            return new PointS((short)(-a.X), (short)(-a.Y));
        }
    }
}