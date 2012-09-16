using System;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO.PacketReader")]
    public class PacketReaderFixture
    {
        private static readonly byte[] ZeroArray = new byte[] { };
        private static readonly byte[] OneArray = new byte[] { 1, };
        private static readonly byte[] TwoArray = new byte[] { 1, 0, };
        private static readonly byte[] FourArray = new byte[] { 1, 0, 0, 0, };
        private static readonly byte[] EightArray = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, };

        #region Throws

        [Test]
        public void ThrowsOnNullBuffer()
        {
            byte[] buffer = null;

            ThrowsAne(() => new PacketReader(buffer));
            ThrowsAne(() => new PacketReader(buffer, 0, 0));
        }

        [Test]
        public void ThrowsOnNullClone()
        {
            PacketReader reader = null;

            ThrowsAne(() => new PacketReader(reader));
        }

        [Test]
        public void ThrowsOnNegativeOffset()
        {
            byte[] buffer = new byte[10];

            ThrowsAoore(() => new PacketReader(buffer, -1, 0));
        }

        [Test]
        public void ThrowsOnNegativeLength()
        {
            byte[] buffer = new byte[10];

            ThrowsAoore(() => new PacketReader(buffer, 0, -1));
        }

        [Test]
        public void ThrowsOnBadSegmentOffset()
        {
            byte[] buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 11, 0));
        }

        [Test]
        public void ThrowsOnBadSegmentLength()
        {
            byte[] buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 0, 11));
        }

        [Test]
        public void ThrowsOnBadSegmentOffsetAndLength()
        {
            byte[] buffer = new byte[10];

            ThrowsAse(() => new PacketReader(buffer, 6, 5));
            ThrowsAse(() => new PacketReader(buffer, 5, 6));
            ThrowsAse(() => new PacketReader(buffer, 0, 11));
            ThrowsAse(() => new PacketReader(buffer, 11, 0));
            ThrowsAse(() => new PacketReader(buffer, 10, 1));
        }

        [Test]
        public void ThrowsOnReadingInZeroLengthSegment()
        {
            var zero = new PacketReader(ZeroArray);
            ThrowsPreOnReadOperations(zero, 1, 13, 10);

            var oneOffset = new PacketReader(OneArray, 1, 0);
            ThrowsPreOnReadOperations(oneOffset, 1, 13, 10);

            var twoOffset = new PacketReader(TwoArray, 2, 0);
            ThrowsPreOnReadOperations(twoOffset, 1, 13, 10);
        }

        [Test]
        public void ThrowsOnReadBytesWithNegativeCount()
        {
            var zero = new PacketReader(ZeroArray);
            ThrowsAoore(() => zero.ReadBytes(-1));
        }

        [Test]
        public void ThrowsOnSkipWithNegativeCount()
        {
            var zero = new PacketReader(ZeroArray);
            ThrowsAoore(() => zero.Skip(-1));
        }

        [Test]
        public void ThrowsOnReadPaddedStringWithNegativePadLength()
        {
            var zero = new PacketReader(ZeroArray);
            ThrowsAoore(() => zero.ReadPaddedString(-1));
        }

        public void ThrowsOnReadPaddedStringWithMissingData()
        {
            var bytes = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, };
            var reader = new PacketReader(bytes);
            ThrowsPre(() => reader.ReadPaddedString(13));
        }

        [Test]
        public void ThrowsOnReadLengthStringWithMissingData()
        {
            var two = new PacketReader(TwoArray);
            ThrowsPre(() => two.ReadLengthString());
        }

        private static void ThrowsPreOnReadOperations(PacketReader r, int skip, int padLength, int byteCount)
        {
            ThrowsPre(() => r.Skip(skip));

            ThrowsPre(() => r.ReadBoolean());

            ThrowsPre(() => r.ReadByte());
            ThrowsPre(() => r.ReadInt16());
            ThrowsPre(() => r.ReadUInt16());
            ThrowsPre(() => r.ReadInt32());
            ThrowsPre(() => r.ReadUInt32());
            ThrowsPre(() => r.ReadInt64());
            ThrowsPre(() => r.ReadUInt64());

            ThrowsPre(() => r.ReadLengthString());
            ThrowsPre(() => r.ReadPaddedString(padLength));
            ThrowsPre(() => r.ReadBytes(byteCount));
        }

        private static void ThrowsPre(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<PacketReadingException>());
        }

        private static void ThrowsAse(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArraySegmentException>());
        }

        private static void ThrowsAne(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentNullException>());
        }

        private static void ThrowsAoore(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        #endregion

        #region Does Not Throw

        [Test]
        public void DoesNotThrowOnNonNullBuffer()
        {
            Assert.That(() => new PacketReader(new byte[10]), Throws.Nothing);
            Assert.That(() => new PacketReader(new byte[10], 0, 10), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnZeroOffsetAndLength()
        {
            Assert.That(() => new PacketReader(ZeroArray, 0, 0), Throws.Nothing);
            Assert.That(() => new PacketReader(new byte[10], 0, 0), Throws.Nothing);
        }

        [Test]
        public void DoesNotThrowOnNonNullClone()
        {
            var reader = new PacketReader(new byte[10]);

            Assert.That(() => new PacketReader(reader), Throws.Nothing);
        }

        #endregion

    }
}
