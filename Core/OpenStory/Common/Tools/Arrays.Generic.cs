using System;

namespace OpenStory.Common
{
    /// <summary>
    /// Generic array helpers!
    /// </summary>
    /// <typeparam name="T">The type of the array.</typeparam>
    public static class Arrays<T>
    {
        /// <summary>
        /// An empty array instance.
        /// </summary>
        public static readonly T[] Empty = new T[0];

        /// <summary>
        /// Creates a reversed copy of the provided array.
        /// </summary>
        /// <param name="array">The array to reverse.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is <see langword="null"/>.</exception>
        /// <returns>a reversed copy of the array.</returns>
        public static T[] Reverse(T[] array)
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
        public static T[] ReverseInPlace(T[] array)
        {
            Guard.NotNull(() => array, array);

            int length = array.Length;
            int half = length / 2;
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
