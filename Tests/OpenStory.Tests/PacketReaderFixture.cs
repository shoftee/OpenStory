using System;
using System.Text;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "IO", Description = "Tests for the OpenStory.Common.IO.PacketReader class")]
    public sealed class PacketReaderFixture
    {
        private static readonly byte[] Empty = new byte[] { };
        private static readonly Random Rng = new Random();

        #region Throws

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
        public void ThrowsOnReadingInZeroLengthSegment()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreOnReadOperations(reader, 1, 13, 10);
        }

        [Test]
        public void ThrowsOnReadingInZeroLengthOffsetSegment()
        {
            var buffer = new byte[] { 1, };
            var reader = new PacketReader(buffer, buffer.Length, 0);

            ThrowsPreOnReadOperations(reader, 1, 13, 10);
        }

        [Test]
        public void ThrowsOnReadBytesWithNegativeCount()
        {
            var reader = new PacketReader(Empty);

            ThrowsAoore(() => reader.ReadBytes(-1));
        }

        [Test]
        public void ThrowsOnSkipWithNegativeCount()
        {
            var reader = new PacketReader(Empty);

            ThrowsAoore(() => reader.Skip(-1));
        }

        [Test]
        public void ThrowsOnReadPaddedStringWithNonPositivePadLength()
        {
            var reader = new PacketReader(Empty);

            ThrowsAoore(() => reader.ReadPaddedString(-1));
        }

        [Test]
        public void ThrowsOnReadPaddedStringWithZeroPadLength()
        {
            var reader = new PacketReader(Empty);

            ThrowsAoore(() => reader.ReadPaddedString(0));
        }

        [Test]
        public void ThrowsOnReadPaddedStringWithMissingData()
        {
            const int ExpectedLength = 13;

            var buffer = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, };
            var reader = new PacketReader(buffer);

            ThrowsPre(() => reader.ReadPaddedString(ExpectedLength));
        }

        [Test]
        public void ThrowsOnReadLengthStringWithMissingData()
        {
            var buffer = new byte[] { 12, 0 }; // Length of 12.
            var reader = new PacketReader(buffer);

            ThrowsPre(() => reader.ReadLengthString());
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

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadBoolean()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadBoolean());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadByte()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadByte());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadInt16()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt16());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadUInt16()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt16());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadInt32()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt32());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadUInt32()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt32());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadInt64()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt64());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadUInt64()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt64());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadBytes()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadBytes(2));
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadLengthString()
        {
            var buffer = new byte[] { 1, 0 }; // Length of 1.
            var reader = new PacketReader(buffer);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadLengthString());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadPaddedString()
        {
            var buffer = new byte[] { 1, 2, 3, };
            var reader = new PacketReader(buffer);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadPaddedString(buffer.Length + 1));
        }

        [Test]
        public void ReadPaddedStringIsReturnedWithoutPadding()
        {
            const int PadLength = 13;
            const string TestString = "shoftee";
            string paddedString = TestString.PadRight(PadLength, '\0');

            var buffer = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(buffer);

            Assert.AreEqual(TestString, reader.ReadPaddedString(PadLength));
        }

        [Test]
        public void ReadPaddedStringIsShorterThanPadding()
        {
            const int PadLength = 13;
            const string TestString = "shoftee_shoftee_shoftee";

            var buffer = Encoding.UTF8.GetBytes(TestString);
            var reader = new PacketReader(buffer);

            Assert.Less(reader.ReadPaddedString(PadLength).Length, PadLength);
        }

        [Test]
        public void ReadLengthStringMatches()
        {
            var buffer = new byte[] { 2, 0, 48, 49 }; // "01"
            var reader = new PacketReader(buffer);

            Assert.AreEqual("01", reader.ReadLengthString());
        }

        [Test]
        public void ReadsBytes()
        {
            const int Count = 100;

            var buffer = GetRandomBytes(Count);
            var reader = new PacketReader(buffer);

            CollectionAssert.AreEqual(buffer, reader.ReadBytes(Count));
        }

        [Test]
        public void ReadsAllBytesOnReadFully()
        {
            const int Count = 100;

            var buffer = GetRandomBytes(Count);
            var reader = new PacketReader(buffer);

            CollectionAssert.AreEqual(buffer, reader.ReadFully());
        }

        private static byte[] GetRandomBytes(int count)
        {
            var buffer = new byte[count];
            Rng.NextBytes(buffer);
            return buffer;
        }

        [Test]
        public void ReadsSingleBytes()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                Assert.AreEqual(b, reader.ReadByte());
            }
        }

        [Test]
        public void ReadsBooleans()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                Assert.AreEqual(b != 0, reader.ReadBoolean());
            }
        }

        [Test]
        public void ReadsLittleEndianInt16Correctly()
        {
            var buffer = new byte[] { 15, 100, };
            var reader = new PacketReader(buffer);

            short actual = reader.ReadInt16();
            short expected = (short)(buffer[0] + buffer[1] * 256);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt16Correctly()
        {
            var buffer = new byte[] { 15, 200, };
            var reader = new PacketReader(buffer);

            ushort actual = reader.ReadUInt16();
            ushort expected = (ushort)(buffer[0] + buffer[1] * 256);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianInt32Correctly()
        {
            var buffer = new byte[] { 123, 23, 23, 123, };
            var reader = new PacketReader(buffer);

            int actual = reader.ReadInt32();
            int expected = (int)
                (buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt32Correctly()
        {
            var buffer = new byte[] { 123, 234, 23, 234, };
            var reader = new PacketReader(buffer);

            uint actual = reader.ReadUInt32();
            uint expected = (uint)
                (buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianInt64Correctly()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            long actual = reader.ReadInt64();
            long expected =
                ((long)buffer[0]) +
                ((long)buffer[1] << 8) +
                ((long)buffer[2] << 16) +
                ((long)buffer[3] << 24) +
                ((long)buffer[4] << 32) +
                ((long)buffer[5] << 40) +
                ((long)buffer[6] << 48) +
                ((long)buffer[7] << 56);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt64Correctly()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            ulong actual = reader.ReadUInt64();
            ulong expected =
                ((ulong)buffer[0]) +
                ((ulong)buffer[1] << 8) +
                ((ulong)buffer[2] << 16) +
                ((ulong)buffer[3] << 24) +
                ((ulong)buffer[4] << 32) +
                ((ulong)buffer[5] << 40) +
                ((ulong)buffer[6] << 48) +
                ((ulong)buffer[7] << 56);

            Assert.AreEqual(expected, actual);

        }

        private static void ThrowsPreAndDoesNotMovePosition(PacketReader reader, TestDelegate action)
        {
            int remainingOld = reader.Remaining;
            ThrowsPre(action);
            Assert.AreEqual(remainingOld, reader.Remaining);
        }

        #endregion

    }
}
