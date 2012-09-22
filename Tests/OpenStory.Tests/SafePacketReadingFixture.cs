using System;
using System.Text;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "PacketReader safe reading tests.")]
    public sealed class SafePacketReadingFixture : PacketReaderFixtureBase
    {
        #region Throws

        [Test]
        public void ReturnsFalseOnReadingInZeroLengthSegment()
        {
            var reader = new PacketReader(Empty);

            GracefullyFailsOnReadOperations(reader, 1, 2, 13, 10);
        }

        [Test]
        public void ReturnsFalseOnReadingInZeroLengthOffsetSegment()
        {
            var buffer = new byte[] { 1, };
            var reader = new PacketReader(buffer, buffer.Length, 0);

            GracefullyFailsOnReadOperations(reader, 1, 2, 13, 10);
        }

        private static void GracefullyFailsOnReadOperations(ISafePacketReader r, int skip, int skipToOffset, int padLength, int byteCount)
        {
            ReturnsFalse(() => r.TrySkipTo(skipToOffset));

            ReturnsFalse(() => r.TrySkip(skip));

            bool @bool;
            ReturnsFalse(() => r.TryReadBoolean(out @bool));

            byte @byte;
            ReturnsFalse(() => r.TryReadByte(out @byte));

            short @short;
            ReturnsFalse(() => r.TryReadInt16(out @short));

            ushort @ushort;
            ReturnsFalse(() => r.TryReadUInt16(out @ushort));

            int @int;
            ReturnsFalse(() => r.TryReadInt32(out @int));

            uint @uint;
            ReturnsFalse(() => r.TryReadUInt32(out @uint));

            long @long;
            ReturnsFalse(() => r.TryReadInt64(out @long));

            ulong @ulong;
            ReturnsFalse(() => r.TryReadUInt64(out @ulong));

            string lengthString;
            ReturnsFalse(() => r.TryReadLengthString(out lengthString));

            string padString;
            ReturnsFalse(() => r.TryReadPaddedString(padLength, out padString));

            byte[] bytes;
            ReturnsFalse(() => r.TryRead(byteCount, out bytes));
        }

        private static void ReturnsFalse(Func<bool> action)
        {
            Assert.IsFalse(action());
        }

        [Test]
        public void ThrowsOnTryReadWithNegativeCount()
        {
            var reader = new PacketReader(Empty);

            byte[] bytes;
            ThrowsAoore(() => reader.TryRead(-1, out bytes));
        }

        [Test]
        public void ThrowsOnTrySkipWithNegativeCount()
        {
            var reader = new PacketReader(Empty);

            ThrowsAoore(() => reader.TrySkip(-1));
        }

        [Test]
        public void ThrowsOnTrySkipToWithBadOffset()
        {
            var buffer = GetRandomBytes(10);
            var reader = new PacketReader(buffer);

            reader.Skip(5);
            ThrowsAoore(() => reader.TrySkipTo(1));
        }

        [Test]
        public void ThrowsOnTrySkipToWithNegativeOffset()
        {
            var buffer = GetRandomBytes(10);
            var reader = new PacketReader(buffer);

            ThrowsAoore(() => reader.TrySkipTo(-1));
        }

        [Test]
        public void ThrowsOnTrySkipToWithBadOffsetInSegment()
        {
            var buffer = GetRandomBytes(20);
            var reader = new PacketReader(buffer, 10, 10);

            reader.Skip(5);
            ThrowsAoore(() => reader.TrySkipTo(1));
        }

        [Test]
        public void ThrowsOnTrySkipToWithNegativeOffsetInSegment()
        {
            var buffer = GetRandomBytes(20);
            var reader = new PacketReader(buffer, 10, 10);

            ThrowsAoore(() => reader.TrySkipTo(-1));
        }

        [Test]
        public void ThrowsOnTryReadPaddedStringWithNonPositivePadLength()
        {
            var reader = new PacketReader(Empty);

            string result;
            ThrowsAoore(() => reader.TryReadPaddedString(-1, out result));
        }

        [Test]
        public void ThrowsOnTryReadPaddedStringWithZeroPadLength()
        {
            var reader = new PacketReader(Empty);

            string result;
            ThrowsAoore(() => reader.TryReadPaddedString(0, out result));
        }

        [Test]
        public void ReturnsFalseOnTryReadPaddedStringWithMissingData()
        {
            const int ExpectedLength = 13;

            var buffer = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, };
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalse(() => reader.TryReadPaddedString(ExpectedLength, out result));
        }

        [Test]
        public void ReturnsFalseOnTryReadLengthStringWithMissingData()
        {
            var buffer = new byte[] { 12, 0 }; // Length of 12.
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalse(() => reader.TryReadLengthString(out result));
        }

        #endregion

        #region Does Not Throw

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadBoolean()
        {
            var reader = new PacketReader(Empty);

            bool result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadBoolean(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadByte()
        {
            var reader = new PacketReader(Empty);

            byte result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadByte(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadInt16()
        {
            var reader = new PacketReader(Empty);

            short result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadInt16(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadUInt16()
        {
            var reader = new PacketReader(Empty);

            ushort result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadUInt16(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadInt32()
        {
            var reader = new PacketReader(Empty);

            int result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadInt32(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadUInt32()
        {
            var reader = new PacketReader(Empty);

            uint result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadUInt32(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadInt64()
        {
            var reader = new PacketReader(Empty);

            long result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadInt64(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadUInt64()
        {
            var reader = new PacketReader(Empty);

            ulong result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadUInt64(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryRead()
        {
            var reader = new PacketReader(Empty);

            byte[] bytes;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryRead(2, out bytes));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadLengthString()
        {
            var buffer = new byte[] { 1, 0 }; // Length of 1.
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadLengthString(out result));
        }

        [Test]
        public void DoesNotMovePositionOnFailureDuringTryReadPaddedString()
        {
            var buffer = new byte[] { 1, 2, 3, };
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalseAndDoesNotMovePosition(reader, () => reader.TryReadPaddedString(buffer.Length + 1, out result));
        }

        private static void ReturnsFalseAndDoesNotMovePosition(PacketReader reader, Func<bool> action)
        {
            int oldRemaining = reader.Remaining;
            Assert.IsFalse(action());
            Assert.AreEqual(oldRemaining, reader.Remaining);
        }

        [Test]
        public void ReadPaddedStringIsReturnedWithoutPadding()
        {
            const int PadLength = 13;
            const string TestString = "shoftee";
            string paddedString = TestString.PadRight(PadLength, '\0');

            var buffer = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(buffer);

            string result;
            bool success = reader.TryReadPaddedString(PadLength, out result);
            Assert.IsTrue(success);
            Assert.AreEqual(TestString, result);
        }

        [Test]
        public void ReadPaddedStringIsShorterThanPadding()
        {
            const int PadLength = 13;
            const string TestString = "shoftee_shoftee_shoftee";

            var buffer = Encoding.UTF8.GetBytes(TestString);
            var reader = new PacketReader(buffer);

            string result;
            bool success = reader.TryReadPaddedString(PadLength, out result);
            Assert.IsTrue(success);
            Assert.Less(result.Length, PadLength);
        }

        [Test]
        public void ReadLengthStringMatches()
        {
            var buffer = new byte[] { 2, 0, 48, 49 }; // "01"
            var reader = new PacketReader(buffer);

            string result;
            bool success = reader.TryReadLengthString(out result);
            Assert.IsTrue(success);
            Assert.AreEqual("01", result);
        }

        [Test]
        public void ReadsBytes()
        {
            const int Count = 100;

            var buffer = GetRandomBytes(Count);
            var reader = new PacketReader(buffer);

            byte[] bytes;
            bool success = reader.TryRead(Count, out bytes);
            Assert.IsTrue(success);
            CollectionAssert.AreEqual(buffer, bytes);
        }

        [Test]
        public void ReadsSingleBytes()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                byte result;
                bool success = reader.TryReadByte(out result);
                Assert.IsTrue(success);
                Assert.AreEqual(b, result);
            }
        }

        [Test]
        public void ReadsBooleans()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                bool result;
                bool success = reader.TryReadBoolean(out result);
                Assert.IsTrue(success);
                Assert.AreEqual(b != 0, result);
            }
        }

        [Test]
        public void ReadsLittleEndianInt16Correctly()
        {
            var buffer = new byte[] { 15, 100, };
            var reader = new PacketReader(buffer);

            short expected = (short)(buffer[0] + buffer[1] * 256);
            short actual;
            bool success = reader.TryReadInt16(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt16Correctly()
        {
            var buffer = new byte[] { 15, 200, };
            var reader = new PacketReader(buffer);

            ushort expected = (ushort)(buffer[0] + buffer[1] * 256);
            ushort actual;
            bool success = reader.TryReadUInt16(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianInt32Correctly()
        {
            var buffer = new byte[] { 123, 23, 23, 123, };
            var reader = new PacketReader(buffer);

            int expected = (int)
                (buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));
            int actual;
            bool success = reader.TryReadInt32(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt32Correctly()
        {
            var buffer = new byte[] { 123, 234, 23, 234, };
            var reader = new PacketReader(buffer);

            uint expected = (uint)
             (buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));
            uint actual;
            bool success = reader.TryReadUInt32(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianInt64Correctly()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            long expected =
                ((long)buffer[0]) +
                ((long)buffer[1] << 8) +
                ((long)buffer[2] << 16) +
                ((long)buffer[3] << 24) +
                ((long)buffer[4] << 32) +
                ((long)buffer[5] << 40) +
                ((long)buffer[6] << 48) +
                ((long)buffer[7] << 56);
            long actual;
            bool success = reader.TryReadInt64(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadsLittleEndianUInt64Correctly()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            ulong expected =
                ((ulong)buffer[0]) +
                ((ulong)buffer[1] << 8) +
                ((ulong)buffer[2] << 16) +
                ((ulong)buffer[3] << 24) +
                ((ulong)buffer[4] << 32) +
                ((ulong)buffer[5] << 40) +
                ((ulong)buffer[6] << 48) +
                ((ulong)buffer[7] << 56);
            ulong actual;
            bool success = reader.TryReadUInt64(out actual);
            Assert.IsTrue(success);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SkipWorksCorrectly()
        {
            var buffer = GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            const int Offset = 10;
            int expected = reader.Remaining - Offset;

            bool success = reader.TrySkip(Offset);
            Assert.IsTrue(success);
            int actual = reader.Remaining;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SkipToWorksCorrectly()
        {
            var buffer = GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            const int ToSkip = 10;
            int expected = reader.Remaining - ToSkip;

            bool success = reader.TrySkipTo(ToSkip);
            Assert.IsTrue(success);
            int actual = reader.Remaining;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}