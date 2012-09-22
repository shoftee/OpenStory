using System;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "PacketReader common tests.")]
    public class PacketReaderFixtureBase
    {
        protected static readonly byte[] Empty = new byte[] { };
        private static readonly Random Rng = new Random();

        protected static void ThrowsAse(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArraySegmentException>());
        }

        protected static void ThrowsAne(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentNullException>());
        }

        protected static void ThrowsAoore(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        protected static byte[] GetRandomBytes(int count)
        {
            var buffer = new byte[count];
            Rng.NextBytes(buffer);
            return buffer;
        }

        [Test]
        public void ThrowsOnNullBuffer()
        {
            ThrowsAne(() => new PacketReader((byte[])null));
        }

        [Test]
        public void ThrowsOnNullBufferWithZeroLengthSegment()
        {
            ThrowsAne(() => new PacketReader(null, 0, 0));
        }

        [Test]
        public void ThrowsOnNullClone()
        {
            ThrowsAne(() => new PacketReader((PacketReader)null));
        }

        [Test]
        public void ThrowsOnNegativeOffset()
        {
            var buffer = new byte[10];

            ThrowsAoore(() => new PacketReader(buffer, -1, 0));
        }

        [Test]
        public void ThrowsOnNegativeLength()
        {
            var buffer = new byte[10];

            ThrowsAoore(() => new PacketReader(buffer, 0, -1));
        }

        [Test]
        public void ThrowsOnBadSegmentOffset()
        {
            var buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 11, 0));
        }

        [Test]
        public void ThrowsOnBadSegmentLength()
        {
            var buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 0, 11));
        }

        [Test]
        public void ThrowsOnBadSegmentOffsetOrLength()
        {
            var buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 6, 5));
            ThrowsAse(() => new PacketReader(buffer, 5, 6));
            ThrowsAse(() => new PacketReader(buffer, 0, 11));
            ThrowsAse(() => new PacketReader(buffer, 11, 0));
            ThrowsAse(() => new PacketReader(buffer, 10, 1));
        }

        [Test]
        public void DoesNotThrowOnNonNullBuffer()
        {
            Assert.That(() => new PacketReader(new byte[10]), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnNonNullBufferWithSegment()
        {
            Assert.That(() => new PacketReader(new byte[10], 2, 6), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnZeroOffsetAndLengthWithEmptyArray()
        {
            Assert.That(() => new PacketReader(Empty, 0, 0), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnZeroOffsetAndLengthWithNonEmptyArray()
        {
            Assert.That(() => new PacketReader(new byte[10], 0, 0), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnNonNullClone()
        {
            var reader = new PacketReader(new byte[10]);

            Assert.That(() => new PacketReader(reader), Throws.Nothing);
        }
    }
}