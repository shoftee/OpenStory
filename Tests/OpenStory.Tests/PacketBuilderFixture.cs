using System;
using NUnit.Framework;
using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "PacketBuilder tests.")]
    sealed class PacketBuilderFixture
    {
        #region Failure

        [Test]
        public void ThrowsOnZeroCapacity()
        {
            Helpers.ThrowsAoore(() => new PacketBuilder(0));
        }

        [Test]
        public void ThrowsOnNegativeCapacity()
        {
            Helpers.ThrowsAoore(() => new PacketBuilder(-1));
        }

        [Test]
        public void ThrowsOdeAfterDisposed()
        {
            var builder = new PacketBuilder();
            builder.Dispose();

            ThrowsOdeOnWriteOperations(builder);
        }

        private static void ThrowsOdeOnWriteOperations(PacketBuilder b)
        {
            Helpers.ThrowsOde(() => b.WriteBoolean(true));

            Helpers.ThrowsOde(() => b.WriteByte(0x77));
            Helpers.ThrowsOde(() => b.WriteInt16(0x7777));
            Helpers.ThrowsOde(() => b.WriteInt16(0x8889));
            Helpers.ThrowsOde(() => b.WriteInt32(0x77777777));
            Helpers.ThrowsOde(() => b.WriteInt32(0x88888889));
            Helpers.ThrowsOde(() => b.WriteInt64(0x7777777777777777));
            Helpers.ThrowsOde(() => b.WriteInt64(0x8888888888888889));
            Helpers.ThrowsOde(() => b.WriteBytes(new byte[] { 123, 43, 32, 123 }));
            Helpers.ThrowsOde(() => b.WriteLengthString("1234"));
            Helpers.ThrowsOde(() => b.WritePaddedString("shoftee", 13));
            Helpers.ThrowsOde(() => b.WriteZeroes(123));
            Helpers.ThrowsOde(() => b.ToByteArray());

            Helpers.ThrowsOde(() => b.WriteVector(new PointS(123, 321)));
        }

        [Test]
        public void ThrowsOnNegativeZeroCount()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAoore(() => builder.WriteZeroes(-1));
        }

        [Test]
        public void ThrowsOnNegativePadding()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAoore(() => builder.WritePaddedString("123", -1));
        }

        [Test]
        public void ThrowsOnZeroPadding()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAoore(() => builder.WritePaddedString("123", 0));
        }

        [Test]
        public void ThrowsOnInsufficientPadding()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAoore(() => builder.WritePaddedString("123", 3));
        }

        [Test]
        public void ThrowsOnNullPaddedString()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAne(() => builder.WritePaddedString(null, 13));
        }

        [Test]
        public void ThrowsOnNullLengthString()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAne(() => builder.WriteLengthString(null));
        }

        [Test]
        public void ThrowsOnNullByteArray()
        {
            var builder = new PacketBuilder();

            Helpers.ThrowsAne(() => builder.WriteBytes(null));
        }

        #endregion

        #region Non-failure

        [Test]
        public void DoesNotThrowOnDefaultCapacity()
        {
            Assert.DoesNotThrow(() => new PacketBuilder());
        }

        [Test]
        public void DoesNotThrowOnPositiveCapacity()
        {
            Assert.DoesNotThrow(() => new PacketBuilder(32));
        }

        [Test]
        public void DoesNotThrowOnDoubleDispose()
        {
            var builder = new PacketBuilder();

            builder.Dispose();
            builder.Dispose();
        }

        [Test]
        public void WritesByte()
        {
            var builder = new PacketBuilder();

            builder.WriteByte(123);
            var array = builder.ToByteArray();
            Assert.AreEqual(123, array[0]);
        }

        [Test]
        public void WritesLittleEndianSignedInt16Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x87, 0x79, };
            builder.WriteInt16(0x7987);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLittleEndianUnsignedInt16Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x87, 0x89, };
            builder.WriteInt16(0x8987);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLittleEndianSignedInt32Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x12, 0x34, 0x87, 0x79, };
            builder.WriteInt32(0x79873412);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLittleEndianUnsignedInt32Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x12, 0x34, 0x87, 0x89, };
            builder.WriteInt32(0x89873412);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLittleEndianSignedInt64Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x87, 0x79, };
            builder.WriteInt64(0x7987341278563412);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLittleEndianUnsignedInt64Correctly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x87, 0x89, };
            builder.WriteInt64(0x8987341278563412);
            var actual = builder.ToByteArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesBooleanCorrectly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x1, 0x0, 0x1, 0x0, 0x1, 0x0 };

            builder.WriteBoolean(true);
            builder.WriteBoolean(false);
            builder.WriteBoolean(true);
            builder.WriteBoolean(false);
            builder.WriteBoolean(true);
            builder.WriteBoolean(false);
            var actual = builder.ToByteArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesLengthStringCorrectly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x02, 0x00, 0x30, 0x31, }; // "01";

            builder.WriteLengthString("01");
            var actual = builder.ToByteArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesPaddedStringCorrectly()
        {
            var builder = new PacketBuilder();

            var expected = new byte[] { 0x31, 0x32, 0x33, 0x34, 0x00 }; // "1234\0";

            const string TestString = "1234";
            const int PadLength = 13;
            builder.WritePaddedString(TestString, PadLength);
            var actual = builder.ToByteArray();

            for(int i = 0; i < PadLength; i ++)
            {
                if (i == TestString.Length)
                {
                    Assert.AreEqual(0, actual[i]);
                }
                else if (i < TestString.Length)
                {
                    Assert.AreEqual((byte)TestString[i], actual[i]);
                }
            }
        }

        [Test]
        public void WritesZeroesCorrectly()
        {
            var builder = new PacketBuilder();
            var expected = new byte[] { 0x12, 0x34, 0, 0, 0, 0, 0, 0x56 };

            builder.WriteByte(0x12);
            builder.WriteByte(0x34);
            builder.WriteZeroes(5);
            builder.WriteByte(0x56);
            var actual = builder.ToByteArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void WritesBytesCorrectly()
        {
            var builder = new PacketBuilder();
            var expected = new byte[] { 0x12, 0x34, 0x56, 0x78, };

            builder.WriteBytes(expected);
            var actual = builder.ToByteArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
