using System;
using System.Text;

namespace OpenStory.Common
{
    /// <summary>
    /// Provides extension methods for manipulating hexadecimal data.
    /// </summary>
    public static class HexExtensions
    {
        /// <summary>
        /// Constructs a byte array from a string of hexadecimal digits.
        /// </summary>
        /// <param name="hex">The string to translate to bytes.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="hex" /> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="hex"/> is not of even length,
        /// OR, if <paramref name="hex"/> contains characters that don't correspond to hexadecimal digits.
        /// </exception>
        /// <returns>the resulting byte array.</returns>
        public static byte[] ToByte(this string hex)
        {
            Guard.NotNull(() => hex, hex);

            if ((hex.Length & 1) != 0)
            {
                throw new ArgumentException(CommonStrings.StringLengthMustBeEven, "hex");
            }

            const string HexDigits = @"0123456789ABCDEF";

            var bytes = new byte[hex.Length >> 1];
            int arrayLength = bytes.Length;
            string uppercase = hex.ToUpperInvariant();
            for (int i = 0; i < arrayLength; i++)
            {
                int index = i << 1;
                int digit = HexDigits.IndexOf(uppercase[index]);
                if (digit == -1)
                {
                    throw new ArgumentException(CommonStrings.StringMustContainOnlyHexDigits, "hex");
                }

                var b = (byte)(digit << 4);

                digit = HexDigits.IndexOf(uppercase[index | 1]);
                if (digit == -1)
                {
                    throw new ArgumentException(CommonStrings.StringMustContainOnlyHexDigits, "hex");
                }

                b |= (byte)digit;
                bytes[i] = b;
            }

            return bytes;
        }

        /// <summary>
        /// Constructs a hexadecimal digit string from a byte array.
        /// </summary>
        /// <param name="array">The byte array to translate to hexadecimal characters.</param>
        /// <param name="hyphenate">Whether to add hyphens between the byte hex.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="array" /> is <see langword="null"/>.</exception>
        /// <returns>the byte array as a hex-digit string.</returns>
        public static string ToHex(this byte[] array, bool hyphenate = false)
        {
            Guard.NotNull(() => array, array);

            var builder = new StringBuilder();
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                if (hyphenate && i > 0)
                {
                    builder.Append('-');
                }

                builder.AppendFormat("{0:X2}", array[i]);
            }

            return builder.ToString();
        }
    }
}
