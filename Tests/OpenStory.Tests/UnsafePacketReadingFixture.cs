using System.Text;
using NUnit.Framework;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "PacketReader unsafe reading tests.")]
    public sealed class UnsafePacketReadingFixture : PacketReaderFixtureBase
    {
        #region Throws

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

        #endregion

        #region Does Not Throw

        [Test]
        public void DoesNotMovePositionOnExceptionDuringReadBoolean()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadBoolean());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringSkip()
        {
            var reader = new PacketReader(Empty);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.Skip(1));
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

            var expected = TestString;
            var actual = reader.ReadPaddedString(PadLength);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadPaddedStringIsShorterThanPadding()
        {
            const int PadLength = 13;
            const string TestString = "shoftee_shoftee_shoftee";

            var buffer = Encoding.UTF8.GetBytes(TestString);
            var reader = new PacketReader(buffer);

            int maximum = PadLength;
            int actual = reader.ReadPaddedString(PadLength).Length;

            Assert.Less(actual, maximum);
        }

        [Test]
        public void ReadLengthStringMatches()
        {
            var buffer = new byte[] { 2, 0, 48, 49 }; // "01"
            var reader = new PacketReader(buffer);

            var expected = "01";
            var actual = reader.ReadLengthString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsBytes()
        {
            const int BufferSize = 100;

            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            var expected = buffer;
            var actual = reader.ReadBytes(BufferSize);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFullyReadsAllBytes()
        {
            const int BufferSize = 100;

            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            var expected = buffer;
            var actual = reader.ReadFully();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFullyReadsAllBytesInSegment()
        {
            const int BufferSize = 100;
            const int Start = 10;
            const int Length = 80;

            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer, Start, Length);

            var expected = buffer.CopySegment(Start, Length);
            var actual = reader.ReadFully();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFullyReadsAllRemainingBytes()
        {
            const int BufferSize = 100;
            const int Offset = 10;
            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            reader.Skip(Offset);

            var expected = buffer.CopySegment(Offset, BufferSize - Offset);
            var actual = reader.ReadFully();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFullyReadsAllRemainingBytesInSegment()
        {
            const int BufferSize = 100;
            const int Start = 10;
            const int Length = 80;
            const int Offset = 10;

            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer, Start, Length);

            reader.Skip(Offset);

            var expected = buffer.CopySegment(Start + Offset, Length - Offset);
            var actual = reader.ReadFully();

            CollectionAssert.AreEqual(expected, actual);            
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

        [Test]
        public void SkipsCorrectly()
        {
            const int BufferSize = 100;
            var buffer = GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            const int SkipLength = 10;

            int oldRemaining = reader.Remaining;
            reader.Skip(SkipLength);
            int newRemaining = reader.Remaining;

            int actualSkipLength = oldRemaining - newRemaining;
            Assert.AreEqual(SkipLength, actualSkipLength);
        }

        #endregion

        private static void ThrowsPre(TestDelegate action)
        {
            Assert.That(action, Throws.TypeOf<PacketReadingException>());
        }

        private static void ThrowsPreAndDoesNotMovePosition(PacketReader reader, TestDelegate action)
        {
            int remainingOld = reader.Remaining;
            ThrowsPre(action);
            Assert.AreEqual(remainingOld, reader.Remaining);
        }
    }
}
