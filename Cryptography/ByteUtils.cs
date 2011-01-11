using System;
using System.Text;

namespace OpenMaple.Cryptography
{
    public static class ByteUtils
    {
        private static readonly Random Random = new Random();
        private const string HexUppercase = "0123456789ABCDEF";
        private const string HexLowercase = "0123456789abcdef";

        /// <summary>
        /// Performs a bit-wise left roll on a byte.
        /// </summary>
        /// <param name="b">The byte to roll left.</param>
        /// <param name="n">The number of bit positions to roll.</param>
        /// <returns>The resulting byte.</returns>
        public static byte RollLeft(byte b, int n)
        {
            int tmp = b << (n & 7);
            return unchecked((byte) (tmp | (tmp >> 8)));
        }

        /// <summary>
        /// Performs a bit-wise right roll on a byte.
        /// </summary>
        /// <param name="b">The byte to roll right.</param>
        /// <param name="n">The number of bit positions to roll.</param>
        /// <returns>The resulting byte.</returns>
        public static byte RollRight(byte b, int n)
        {
            int tmp = b << (8 - (n & 7));
            return unchecked((byte) (tmp | (tmp >> 8)));
        }

        /// <summary>
        /// Constructs a byte array from a string of hexadecimal digits.
        /// </summary>
        /// <param name="str">The string to translate to bytes.</param>
        /// <exception cref="ArgumentException">The exception is thrown 
        /// if <paramref name="str"/> is not of even length,
        /// OR,
        /// if <paramref name="str"/> contains characters that don't correspond to hexadecimal digits.
        /// </exception>
        /// <returns>The resulting byte array.</returns>
        public static byte[] HexToByte(string str)
        {
            if ((str.Length & 1) != 0)
            {
                throw new ArgumentException("The string must have event length", "str");
            }
            byte[] bytes = new byte[str.Length >> 1];
            int arrayLength = bytes.Length;
            string uppercase = str.ToUpperInvariant();
            for (int i = 0; i < arrayLength; i++)
            {
                int index = i << 1;
                int position = HexUppercase.IndexOf(uppercase[index]);
                if (position == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "str");
                }
                byte b = (byte) (position << 4);

                position = HexUppercase.IndexOf(uppercase[index | 1]);
                if (position == -1)
                {
                    throw new ArgumentException("The string must consist only of hex digits.", "str");
                }
                b |= (byte) position;
                bytes[i] = b;
            }
            return bytes;
        }

        /// <summary>Constructs a hexadecimal digit string from a byte array.</summary>
        /// <param name="bytes">The byte array to translate to hexadecimal characters.</param>
        /// <param name="lowercase">Whether to use lowercase or uppercase hexadecimal characters.</param>
        /// <returns>The byte array as a hex-digit string.</returns>
        public static string ByteToHex(byte[] bytes, bool lowercase = false)
        {
            string hex = lowercase ? HexLowercase : HexUppercase;
            var builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(hex[b >> 4]);
                builder.Append(hex[b & 0x0F]);
            }
            return builder.ToString();
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
            do number = Random.Next();
            while (number != 0);

            byte[] iv = BitConverter.GetBytes(number);
            return iv;
        }
    }
}
