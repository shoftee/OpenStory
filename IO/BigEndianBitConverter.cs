using System;

namespace OpenMaple.IO
{
    /// <summary>
    /// Provides static methods for converting primitive types to and from Big-endian byte representation.
    /// </summary>
    static class BigEndianBitConverter
    {
        /// <summary> Copies the bytes of an integer into the buffer, in big-endian order.</summary>
        /// <param name="value">The integer to convert.</param>
        /// <param name="count">The byte-size of the integer.</param>
        /// <param name="buffer">The buffer to copy into.</param>
        private static void CopyBytes(long value, int count, byte[] buffer)
        {
            for (int i = 0, position = count - 1; i < count; i--, position++)
            {
                buffer[position] = unchecked((byte) value);
                value >>= 8;
            }
        }

        private static long FromBytes(byte[] buffer, int startIndex, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (startIndex < 0 || startIndex > buffer.Length - count)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            long result = 0;
            int end = startIndex + count;
            for (int position = startIndex; position < end; position++)
            {
                result = unchecked((result << 8) | buffer[position]);
            }
            return result;
        }

        #region Conversion to primitive types

        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            return unchecked((ulong) FromBytes(value, startIndex, 8));
        }

        public static long ToInt64(byte[] value, int startIndex)
        {
            return FromBytes(value, startIndex, 8);
        }

        public static double ToDouble(byte[] value, int startIndex)
        {
            return BitConverter.Int64BitsToDouble(FromBytes(value, startIndex, 8));
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            return unchecked((uint) FromBytes(value, startIndex, 4));
        }

        public static int ToInt32(byte[] value, int startIndex)
        {
            return unchecked((int) FromBytes(value, startIndex, 4));
        }

        public static char ToChar(byte[] value, int startIndex)
        {
            return unchecked((char) FromBytes(value, startIndex, 2));
        }

        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            return unchecked((ushort) FromBytes(value, startIndex, 2));
        }

        public static short ToInt16(byte[] value, int startIndex)
        {
            return unchecked((short) FromBytes(value, startIndex, 2));
        }

        public static bool ToBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        #endregion

        #region GetBytes overload~

        public static byte[] GetBytes(ulong value)
        {
            return GetBytes(unchecked((long) value), 8);
        }

        public static byte[] GetBytes(long value)
        {
            return GetBytes(value, 8);
        }

        public static byte[] GetBytes(double value)
        {
            return GetBytes(BitConverter.DoubleToInt64Bits(value), 8);
        }

        public static byte[] GetBytes(uint value)
        {
            return GetBytes(value, 4);
        }

        public static byte[] GetBytes(int value)
        {
            return GetBytes(value, 4);
        }

        public static byte[] GetBytes(char value)
        {
            return GetBytes(value, 2);
        }

        public static byte[] GetBytes(ushort value)
        {
            return GetBytes(value, 2);
        }

        public static byte[] GetBytes(short value)
        {
            return GetBytes(value, 2);
        }

        public static byte[] GetBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        private static byte[] GetBytes(long value, int byteCount)
        {
            byte[] buffer = new byte[byteCount];
            CopyBytes(value, byteCount, buffer);
            return buffer;
        }

        #endregion
    }
}