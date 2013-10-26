using System;
using OpenStory.Common.IO;

namespace OpenStory.Common
{
    /// <summary>
    /// Extensions for arrays and stuff.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Clones a byte array.
        /// </summary>
        /// <param name="array">The array to clone.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <returns>the new array.</returns>
        public static byte[] FastClone(this byte[] array)
        {
            Guard.NotNull(() => array, array);

            int length = array.Length;
            var newArray = new byte[length];
            Buffer.BlockCopy(array, 0, newArray, 0, length);
            return newArray;
        }

        /// <summary>
        /// Extracts a segment from a given array.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="offset">The start of the segment.</param>
        /// <param name="length">The length of the segment.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="offset"/> or <paramref name="length"/> are negative.
        /// </exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if the array segment defined by <paramref name="offset"/> and <paramref name="length"/>
        /// does not fit the bounds of the provided array.
        /// </exception>
        /// <returns>a copy of the segment.</returns>
        public static byte[] CopySegment(this byte[] array, int offset, int length)
        {
            Guard.NotNull(() => array, array);

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, CommonStrings.OffsetMustBeNonNegative);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, CommonStrings.LengthMustBeNonNegative);
            }

            if (array.Length <= offset || array.Length < offset + length)
            {
                throw ArraySegmentException.GetByStartAndLength(offset, length);
            }

            var segment = new byte[length];
            Buffer.BlockCopy(array, offset, segment, 0, length);
            return segment;
        }

        /// <summary>
        /// Creates a reversed copy of the provided array.
        /// </summary>
        /// <param name="array">The array to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <returns>a reversed copy of the array.</returns>
        public static T[] Reverse<T>(this T[] array)
        {
            Guard.NotNull(() => array, array);

            int length = array.Length;

            var copy = new T[length];
            for (int i = 0; i < length; i++)
            {
                int mirrorIndex = length - i - 1;

                copy[i] = array[mirrorIndex];
            }

            return copy;
        }

        /// <summary>
        /// Reverses the provided array in-place.
        /// </summary>
        /// <param name="array">The array to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <returns>the same instance.</returns>
        public static T[] ReverseInPlace<T>(this T[] array)
        {
            Guard.NotNull(() => array, array);

            int length = array.Length;
            int half = length >> 1;
            for (int i = 0; i < half; i++)
            {
                int mirrorIndex = length - i - 1;

                T value = array[i];
                array[i] = array[mirrorIndex];
                array[mirrorIndex] = value;
            }

            return array;
        }
    }
}