using System.Net;

namespace OpenStory.Common.IO
{
    static class ByteOrder
    {
        public static void NetworkToHostOrder(ref short number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        public static void NetworkToHostOrder(ref ushort number)
        {
            number = (ushort) IPAddress.NetworkToHostOrder((short) number);
        }

        public static void NetworkToHostOrder(ref int number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        public static void NetworkToHostOrder(ref uint number)
        {
            number = (uint) IPAddress.NetworkToHostOrder((int) number);
        }

        public static void NetworkToHostOrder(ref long number)
        {
            number = IPAddress.NetworkToHostOrder(number);
        }

        public static void NetworkToHostOrder(ref ulong number)
        {
            number = (ulong) IPAddress.NetworkToHostOrder((long) number);
        }

        public static void HostToNetworkOrder(ref short number)
        {
            number = IPAddress.HostToNetworkOrder(number);
        }
    }
}
