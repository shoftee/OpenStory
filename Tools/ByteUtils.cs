using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    static class ByteUtils
    {
        // Rolls input's bytes to the left, count times.
        public static byte RollLeft(byte input, int count)
        {
            int tmp = (int) input & 0xFF;
            tmp <<= (count % 8);
            return (byte) ((tmp & 0xFF) | (tmp >> 8));
        }

        // Rolls input's bytes to the right, count times.
        public static byte RollRight(byte input, int count)
        {
            uint tmp = (uint) input & 0xFF;
            tmp = (tmp << 8) >> (count % 8);//ZFRS((tmp << 8), count % 8);
            return (byte) ((tmp & 0xFF) | (tmp >> 8));//ZFRS(tmp, 8));
        }

        // Makes an array that contains the first count bytes in input repeated mul times.
        public static byte[] MultiplyBytes(byte[] input, int count, int mul)
        {
            int length = count * mul;
            byte[] ret = new byte[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = input[i % count];
            }
            return ret;
        }

        // Zero-fill right shift, deprecated
        public static int ZeroFillRightShift(int i, int j)
        {
            return (int) BitConverter.ToUInt32(BitConverter.GetBytes(i >> j), 0);
        }

    }
}
