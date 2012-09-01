using System;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides convenient methods for manipulating binary data.
    /// </summary>
    public static class ByteHelpers
    {
        /// <summary>
        /// Constructs a byte array from a string of hexadecimal digits.
        /// </summary>
        /// <param name="hex">The string to translate to bytes.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hex" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hex"/> is not of even length,
        /// OR, if <paramref name="hex"/> contains characters that don't correspond to hexadecimal digits.
        /// </exception>
        /// <returns>the resulting byte array.</returns>
        public static byte[] ToByte(this string hex)
        {
            if (hex == null) throw new ArgumentNullException("hex");
            if ((hex.Length & 1) != 0)
            {
                throw new ArgumentException("The string must have event length", "hex");
            }

            const string HexDigits = "0123456789ABCDEF";

            var bytes = new byte[hex.Length >> 1];
            int arrayLength = bytes.Length;
            string uppercase = hex.ToUpperInvariant();
            for (int i = 0; i < arrayLength; i++)
            {
                int index = i << 1;
                int digit = HexDigits.IndexOf(uppercase[index]);
                if (digit == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "hex");
                }
                var b = (byte) (digit << 4);

                digit = HexDigits.IndexOf(uppercase[index | 1]);
                if (digit == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "hex");
                }
                b |= (byte) digit;
                bytes[i] = b;
            }
            return bytes;
        }

        /// <summary>
        /// Constructs a hexadecimal digit string from a byte array.
        /// </summary>
        /// <param name="array">The byte array to translate to hexadecimal characters.</param>
        /// <param name="lowercase">Whether to use lowercase or uppercase hexadecimal characters.</param>
        /// <returns>the byte array as a hex-digit string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
        public static string ToHex(this byte[] array, bool lowercase = false)
        {
            if (array == null) throw new ArgumentNullException("array");

            var builder = new StringBuilder();
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                builder.AppendFormat("{0:X2}", array[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Clones a byte array.
        /// </summary>
        /// <param name="array">The array to clone.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="array"/> is <c>null</c>.
        /// </exception>
        /// <returns>the new array.</returns>
        public static byte[] FastClone(this byte[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            int length = array.Length;
            var newArray = new byte[length];
            Buffer.BlockCopy(array, 0, newArray, 0, length);
            return newArray;
        }

        /// <summary>
        /// Extracts a segment from a given array.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="start">The start of the segment.</param>
        /// <param name="length">The length of the segment.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="array"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="start"/> is negative or outside the range of the array, 
        /// or if <paramref name="length"/> is negative or the segment ends outside the array's bounds.</exception>
        /// <returns>a copy of the segment.</returns>
        public static byte[] Segment(this byte[] array, int start, int length)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (start < 0 || array.Length <= start)
            {
                throw new ArgumentOutOfRangeException("start");
            }
            if (length < 0 || array.Length < start + length)
            {
                throw new ArgumentOutOfRangeException("length");
            }
            var segment = new byte[length];
            Buffer.BlockCopy(array, start, segment, 0, length);
            return segment;
        }
    }
}