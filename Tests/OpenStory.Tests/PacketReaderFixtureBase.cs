using System;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "PacketReader common tests.")]
    class PacketReaderFixtureBase
    {
        [Test]
        public void ThrowsOnNullBuffer()
        {
            Helpers.ThrowsAne(() => new PacketReader((byte[])null));
        }

        [Test]
        public void ThrowsOnNullBufferWithZeroLengthSegment()
        {
            Helpers.ThrowsAne(() => new PacketReader(null, 0, 0));
        }

        [Test]
        public void ThrowsOnNullClone()
        {
            Helpers.ThrowsAne(() => new PacketReader((PacketReader)null));
        }

        [Test]
        public void ThrowsOnNegativeOffset()
        {
            var buffer = new byte[10];

            Helpers.ThrowsAoore(() => new PacketReader(buffer, -1, 0));
        }

        [Test]
        public void ThrowsOnNegativeLength()
        {
            var buffer = new byte[10];

            Helpers.ThrowsAoore(() => new PacketReader(buffer, 0, -1));
        }

        [Test]
        public void ThrowsOnBadSegmentOffset()
        {
            var buffer = new byte[10];

            Helpers.ThrowsAse(() => new PacketReader(buffer, 11, 0));
        }

        [Test]
        public void ThrowsOnBadSegmentLength()
        {
            var buffer = new byte[10];

            Helpers.ThrowsAse(() => new PacketReader(buffer, 0, 11));
        }

        [Test]
        public void ThrowsOnBadSegmentOffsetOrLength()
        {
            var buffer = new byte[10];

            Helpers.ThrowsAse(() => new PacketReader(buffer, 6, 5));
            Helpers.ThrowsAse(() => new PacketReader(buffer, 5, 6));
            Helpers.ThrowsAse(() => new PacketReader(buffer, 0, 11));
            Helpers.ThrowsAse(() => new PacketReader(buffer, 11, 0));
            Helpers.ThrowsAse(() => new PacketReader(buffer, 10, 1));
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
            Assert.That(() => new PacketReader(Helpers.Empty, 0, 0), Throws.Nothing);
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