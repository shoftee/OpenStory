using System.Net;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides static helper methods for changing the byte order of numbers.
    /// </summary>
    public static class ByteOrder
    {
        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref short number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref ushort number)
        {
            number = (ushort)IPAddress.NetworkToHostOrder((short)number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref int number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref uint number)
        {
            number = (uint)IPAddress.NetworkToHostOrder((int)number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref long number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from network order (big-endian) to host order.
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void NetworkToHostOrder(ref ulong number)
        {
            number = (ulong)IPAddress.NetworkToHostOrder((long)number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref short number)
        {
            number = IPAddress.HostToNetworkOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref ushort number)
        {
            number = (ushort)IPAddress.HostToNetworkOrder((short)number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref int number)
        {
            number = IPAddress.HostToNetworkOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref uint number)
        {
            number = (uint)IPAddress.HostToNetworkOrder((int)number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref long number)
        {
            number = IPAddress.HostToNetworkOrder(number);
        }

        /// <summary>
        /// Converts the byte order of the specified number from host order to network order (big-endian).
        /// </summary>
        /// <param name="number">The number to convert.</param>
        public static void HostToNetworkOrder(ref ulong number)
        {
            number = (ulong)IPAddress.HostToNetworkOrder((long)number);
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref ulong number)
        {
            number = (ulong)FlipNumber((long)number, 8);
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref long number)
        {
            number = FlipNumber(number, 8);
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref uint number)
        {
            number = unchecked((uint)FlipNumber(number, 4));
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref int number)
        {
            number = unchecked((int)FlipNumber(number, 4));
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref ushort number)
        {
            number = unchecked((ushort)FlipNumber(number, 2));
        }

        /// <summary>
        /// Flips the bytes of the specified number.
        /// </summary>
        /// <param name="number">The number to flip the bytes of.</param>
        public static void FlipBytes(ref short number)
        {
            number = unchecked((short)FlipNumber(number, 2));
        }

        private static long FlipNumber(long number, int byteCount)
        {
            int i = 0;
            long oldNumber = number;
            long newNumber = 0;
            while (i++ < byteCount)
            {
                newNumber <<= 8;
                newNumber |= oldNumber & 0xFF;
                oldNumber >>= 8;
            }
            number = newNumber;
            return number;
        }
    }
}
