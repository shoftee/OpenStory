using System;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    static internal class Helpers
    {
        public static readonly byte[] Empty = new byte[] { };
        private static readonly Random Rng = new Random();

        public static void ThrowsAse(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArraySegmentException>());
        }

        public static void ThrowsAne(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentNullException>());
        }

        public static void ThrowsAoore(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        public static void ThrowsOde(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ObjectDisposedException>());
        }

        public static byte[] GetRandomBytes(int count)
        {
            var buffer = new byte[count];
            Rng.NextBytes(buffer);
            return buffer;
        }
    }
}