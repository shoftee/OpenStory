using System;
using System.Security.Cryptography;

namespace OpenStory.Tests.Helpers
{
    public static class Helpers
    {
        public static readonly byte[] EmptyBuffer = new byte[0];

        private static readonly RandomNumberGenerator Rng = new RNGCryptoServiceProvider();

        public static byte[] GetRandomBytes(int count)
        {
            var buffer = new byte[count];
            Rng.GetBytes(buffer);
            return buffer;
        }
    }
}