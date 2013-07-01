using System;

namespace OpenStory.Tests.Common
{
    static internal class Helpers
    {
        public static readonly byte[] Empty = new byte[] { };

        public static byte[] GetRandomBytes(int count)
        {
            var buffer = new byte[count];
            new Random().NextBytes(buffer);
            return buffer;
        }
    }
}