using System;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides convenient methods for manipulating binary data.
    /// </summary>
    public static class ByteHelpers
    {
        private const string HexUppercase = "0123456789ABCDEF";
        private const string HexLowercase = "0123456789abcdef";
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Performs a bit-wise left roll on a byte.
        /// </summary>
        /// <param name="b">The byte to roll left.</param>
        /// <param name="count">The number of bit positions to roll.</param>
        /// <returns>The resulting byte.</returns>
        public static byte RollLeft(byte b, int count)
        {
            int tmp = b << (count & 7);
            return unchecked((byte) (tmp | (tmp >> 8)));
        }

        /// <summary>
        /// Performs a bit-wise right roll on a byte.
        /// </summary>
        /// <param name="b">The byte to roll right.</param>
        /// <param name="count">The number of bit positions to roll.</param>
        /// <returns>The resulting byte.</returns>
        public static byte RollRight(byte b, int count)
        {
            int tmp = b << (8 - (count & 7));
            return unchecked((byte) (tmp | (tmp >> 8)));
        }

        /// <summary>
        /// Constructs a byte array from a string of hexadecimal digits.
        /// </summary>
        /// <param name="hex">The string to translate to bytes.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hex" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hex"/> is not of even length,
        /// OR, if <paramref name="hex"/> contains characters that don't correspond to hexadecimal digits.
        /// </exception>
        /// <returns>The resulting byte array.</returns>
        public static byte[] HexToByte(string hex)
        {
            if (hex == null) throw new ArgumentNullException("hex");
            if ((hex.Length & 1) != 0)
            {
                throw new ArgumentException("The string must have event length", "hex");
            }
            var bytes = new byte[hex.Length >> 1];
            int arrayLength = bytes.Length;
            string uppercase = hex.ToUpperInvariant();
            for (int i = 0; i < arrayLength; i++)
            {
                int index = i << 1;
                int position = HexUppercase.IndexOf(uppercase[index]);
                if (position == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "hex");
                }
                var b = (byte) (position << 4);

                position = HexUppercase.IndexOf(uppercase[index | 1]);
                if (position == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "hex");
                }
                b |= (byte) position;
                bytes[i] = b;
            }
            return bytes;
        }

        /// <summary>
        /// Constructs an uppercase hexadecimal digit string from a byte array.
        /// </summary>
        /// <param name="array">The byte array to translate to hexadecimal characters.</param>
        /// <returns>The byte array as a hex-digit string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
        public static string ByteToHex(byte[] array)
        {
            return ByteToHex(array, false);
        }

        /// <summary>
        /// Constructs a hexadecimal digit string from a byte array.
        /// </summary>
        /// <param name="array">The byte array to translate to hexadecimal characters.</param>
        /// <param name="lowercase">Whether to use lowercase or uppercase hexadecimal characters.</param>
        /// <returns>The byte array as a hex-digit string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <c>null</c>.</exception>
        public static string ByteToHex(byte[] array, bool lowercase)
        {
            if (array == null) throw new ArgumentNullException("array");

            string hex = lowercase ? HexLowercase : HexUppercase;
            var builder = new StringBuilder();
            foreach (byte b in array)
            {
                builder.Append(hex[b >> 4]);
                builder.Append(hex[b & 0x0F]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Extracts a segment from a given array.
        /// </summary>
        /// <param name="array">The source array.</param>
        /// <param name="start">The start of the segment.</param>
        /// <param name="length">The length of the segment.</param>
        /// <returns>A copy of the segment.</returns>
        public static byte[] SegmentFrom(byte[] array, int start, int length)
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
            byte[] segment = new byte[length];
            Buffer.BlockCopy(array, start, segment, 0, length);
            return segment;
        }

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>A 4-byte IV array.</returns>
        public static byte[] GetNewIV()
        {
            // Just in case we hit that 1 in 2147483648 chance.
            // Things go very bad if the IV is 0.
            int number;
            do number = Rng.Next();
            while (number == 0);

            byte[] iv = BitConverter.GetBytes(number);
            return iv;
        }
    }
}