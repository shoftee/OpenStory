using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    static class ByteUtils
    {
        private static readonly Random Rng = new Random();

        // Rolls input's bytes to the left, n times.
        public static byte RollLeft(byte b, int n)
        {
            int tmp = b & 0xFF;
            tmp <<= (n % 8);
            return (byte) ((tmp & 0xFF) | (tmp >> 8));
        }

        // Rolls input's bytes to the right, n times.
        public static byte RollRight(byte b, int n)
        {
            uint tmp = (uint) b & 0xFF;
            tmp = (tmp << 8) >> (n % 8);
            return (byte) ((tmp & 0xFF) | (tmp >> 8));
        }

        // Makes an array that contains the first count bytes in input repeated mul times.
        public static byte[] MultiplyBytes(byte[] input, int byteCount, int repeat)
        {
            int length = byteCount * repeat;
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                bytes[i] = input[i % byteCount];
            }
            return bytes;
        }

        // Zero-fill right shift, deprecated
        public static int ZeroFillRightShift(int i, int j)
        {
            return (int) BitConverter.ToUInt32(BitConverter.GetBytes(i >> j), 0);
        }

        public static byte[] HexToByte(string str)
        {
            if (str.Length % 2 != 0)
            {
                throw new ArgumentException("The string must have even length.", "str");
            }
            byte[] bytes = new byte[str.Length / 2];

            string uppercase = str.ToUpperInvariant();
            for (int i = 0; i < bytes.Length; i++)
            {
                char first = uppercase[i * 2], second = uppercase[i * 2 + 1];
                byte d = 0;

                if ('0' <= first && first <= '9') d |= (byte) ((first - '0') << 4);
                else if ('A' <= first && first <= 'F') d |= (byte) ((first - 'A' + 10) << 4);
                else throw new ArgumentException("The string must consist only of hex digits.", "str");

                if ('0' <= second && second <= '9') d |= (byte) (second - '0');
                else if ('A' <= second && second <= 'F') d |= (byte) (second - 'A' + 10);
                else throw new ArgumentException("The string must consist only of hex digits.", "str");
                bytes[i] = d;
            }
            return bytes;
        }

        public static string ByteToHex(byte[] bytes)
        {
            char[] hex =
            { 
                '0', '1', '2', '3', '4', '5', '6', '7', 
                '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' 
            };
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(hex[(b & 0xF0) >> 4]);
                builder.Append(hex[b & 0x0F]);
            }
            return builder.ToString();
        }

        public static byte[] StringToByte(string str)
        {
            byte[] data = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                data[i] = (byte) str[i];
            }
            return data;
        }

        public static byte[] GetFreshIv()
        {
            // Just in case we hit that 1 in 2^32 chance F3
            int number;
            do
            {
                number = Rng.Next();
            } while (number != 0);

            var iv = BitConverter.GetBytes(number);
            return iv;
        }

    }
}
