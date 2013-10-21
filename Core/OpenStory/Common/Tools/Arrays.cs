using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.IO;

namespace OpenStory.Common
{
    /// <summary>
    /// Array helpers!
    /// </summary>
    public static class Arrays
    {
        /// <summary>
        /// Concatenates the provided byte arrays.
        /// </summary>
        /// <param name="arrays">The arrays to concatenate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arrays"/> is <see langword="null"/>.</exception>
        /// <returns>the resulting array.</returns>
        public static byte[] FastJoin(IList<byte[]> arrays)
        {
            Guard.NotNull(() => arrays, arrays);

            return FastJoinList(arrays);
        }

        /// <summary>
        /// Concatenates the provided byte arrays.
        /// </summary>
        /// <param name="arrays">The arrays to concatenate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="arrays"/> is <see langword="null"/>.</exception>
        /// <returns>the resulting array.</returns>
        public static byte[] FastJoin(params byte[][] arrays)
        {
            Guard.NotNull(() => arrays, arrays);

            return FastJoinList(arrays);
        }

        private static byte[] FastJoinList(IList<byte[]> arrays)
        {
            var totalLength = arrays.Sum(s => s.Length);
            var buffer = new byte[totalLength];

            int offset = 0;
            foreach (var array in arrays)
            {
                var count = array.Length;
                Buffer.BlockCopy(array, 0, buffer, offset, count);
                offset += count;
            }

            return buffer;
        }
    }
}
