using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO.PacketReader")]
    public class PacketReaderFixture
    {
        private static readonly byte[] ZeroArray = new byte[] { };
        private static readonly byte[] OneArray = new byte[] { 1, };
        private static readonly byte[] TwoArray = new byte[] { 1, 2, };
        private static readonly byte[] FourArray = new byte[] { 1, 2, 3, 4, };
        private static readonly byte[] EightArray = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, };
        private static readonly byte[] SixteenArray = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, };

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
        public void ThrowsOnReadPaddedStringWithNonPositivePadLength()
        {
            var zero = new PacketReader(ZeroArray);
            ThrowsAoore(() => zero.ReadPaddedString(-1));
            ThrowsAoore(() => zero.ReadPaddedString(0));
        }

        [Test]
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

        [Test]
        public void DoesNotMovePositionOnExceptionDuringByteRead()
        {
            var reader = new PacketReader(ZeroArray);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadBoolean());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadByte());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt16());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt16());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt32());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt32());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadInt64());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadUInt64());
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadBytes(2));
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringLengthStringRead()
        {
            var lengthString = new byte[] { 1, 0 };

            var reader = new PacketReader(lengthString);

            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadLengthString());
        }

        [Test]
        public void DoesNotMovePositionOnExceptionDuringPaddedStringRead()
        {
            var paddedString = new byte[] { 1, 2, 3, };

            var reader = new PacketReader(paddedString);
            ThrowsPreAndDoesNotMovePosition(reader, () => reader.ReadPaddedString(paddedString.Length + 1));
        }

        [Test]
        public void ReadPaddedStringIsReturnedWithoutPadding()
        {
            int length = 13;
            var testString = "shoftee";
            string paddedString = testString.PadRight(length, '\0');
            var bytes = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(bytes);
            Assert.AreEqual(testString, reader.ReadPaddedString(length));
        }

        [Test]
        public void ReadPaddedStringIsShorterThanPadding()
        {
            int length = 13;
            string testString = "shoftee_shoftee_shoftee";
            var bytes = Encoding.UTF8.GetBytes(testString);
            var reader = new PacketReader(bytes);
            Assert.Less(reader.ReadPaddedString(length).Length, length);
        }

        [Test]
        public void ReadLengthStringMatches()
        {
            var bytes = new byte[] { 2, 0, 48, 49 }; // "01"

            var reader = new PacketReader(bytes);
            Assert.AreEqual("01", reader.ReadLengthString());
        }

        [Test]
        public void ReadsBytes()
        {
            var reader = new PacketReader(EightArray);

            CollectionAssert.AreEqual(EightArray, reader.ReadBytes(EightArray.Length));
        }

        [Test]
        public void ReadsAllBytesOnReadFully()
        {
            var bytes = new byte[100];
            new Random().NextBytes(bytes);

            var reader = new PacketReader(bytes);

            CollectionAssert.AreEqual(bytes, reader.ReadFully());
        }

        [Test]
        public void ReadsSingleBytes()
        {
            var bytes = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };

            var reader = new PacketReader(bytes);

            foreach (var b in bytes)
            {
                Assert.AreEqual(b, reader.ReadByte());
            }
        }

        [Test]
        public void ReadsBooleans()
        {
            var bytes = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };

            var reader = new PacketReader(bytes);

            foreach (var b in bytes)
            {
                Assert.AreEqual(b != 0, reader.ReadBoolean());
            }
        }

        [Test]
        public void ReadsLittleEndian16BitInteger()
        {
            var reader = new PacketReader(FourArray);

            short s = reader.ReadInt16();
            short expectedShort = (short)(FourArray[0] + FourArray[1] * 256);
            Assert.AreEqual(expectedShort, s);

            ushort u = reader.ReadUInt16();
            ushort expectedUshort = (ushort)(FourArray[2] + FourArray[3] * 256);
            Assert.AreEqual(expectedUshort, u);
        }

        [Test]
        public void ReadsLittleEndian32BitInteger()
        {
            var reader = new PacketReader(EightArray);

            int i = reader.ReadInt32();
            int expectedInt = (int)
                (EightArray[0] + (EightArray[1] << 8) + (EightArray[2] << 16) + (EightArray[3] << 24));
            Assert.AreEqual(expectedInt, i);

            uint u = reader.ReadUInt32();
            uint expectedUint = (uint)
                (EightArray[4] + (EightArray[5] << 8) + (EightArray[6] << 16) + (EightArray[7] << 24));
            Assert.AreEqual(expectedUint, u);
        }

        [Test]
        public void ReadsLittleEndian64BitInteger()
        {
            var reader = new PacketReader(SixteenArray);

            long l = reader.ReadInt64();
            long expectedLong = ((long)SixteenArray[0]) +
                                ((long)SixteenArray[1] << 8) +
                                ((long)SixteenArray[2] << 16) +
                                ((long)SixteenArray[3] << 24) +
                                ((long)SixteenArray[4] << 32) +
                                ((long)SixteenArray[5] << 40) +
                                ((long)SixteenArray[6] << 48) +
                                ((long)SixteenArray[7] << 56);
            Assert.AreEqual(expectedLong, l);

            ulong u = reader.ReadUInt64();
            ulong expectedUlong = ((ulong)SixteenArray[8]) +
                                  ((ulong)SixteenArray[9] << 8) +
                                  ((ulong)SixteenArray[10] << 16) +
                                  ((ulong)SixteenArray[11] << 24) +
                                  ((ulong)SixteenArray[12] << 32) +
                                  ((ulong)SixteenArray[13] << 40) +
                                  ((ulong)SixteenArray[14] << 48) +
                                  ((ulong)SixteenArray[15] << 56);

            Assert.AreEqual(expectedUlong, u);
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
