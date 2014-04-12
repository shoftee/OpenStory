using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common.IO
{
    [Category("OpenStory.Common.IO.PacketReader.Safe")]
    [TestFixture]
    public sealed class SafePacketReadingFixture : PacketReaderFixtureBase
    {
        private static PacketReader EmptyReader
        {
            get { return new PacketReader(Helpers.EmptyBuffer); }
        }

        #region Failure

        [Test]
        public void Should_Return_False_On_Reading_In_Zero_Length_Segment()
        {
            GracefullyFailsOnReadOperations(EmptyReader, 1, 2, 13, 10);
        }

        [Test]
        public void Should_Return_False_On_Reading_In_Offset_Zero_Length_Segment()
        {
            var buffer = new byte[] { 1, };
            var reader = new PacketReader(buffer, buffer.Length, 0);

            GracefullyFailsOnReadOperations(reader, 1, 2, 13, 10);
        }

        private static void GracefullyFailsOnReadOperations(ISafePacketReader r, int skip, int skipToOffset, int padLength, int byteCount)
        {
            r.TrySkipTo(skipToOffset).Should().BeFalse();

            r.TrySkip(skip).Should().BeFalse();

            bool @bool;
            r.TryReadBoolean(out @bool).Should().BeFalse();

            byte @byte;
            r.TryReadByte(out @byte).Should().BeFalse();

            short @short;
            r.TryReadInt16(out @short).Should().BeFalse();

            ushort @ushort;
            r.TryReadUInt16(out @ushort).Should().BeFalse();

            int @int;
            r.TryReadInt32(out @int).Should().BeFalse();

            uint @uint;
            r.TryReadUInt32(out @uint).Should().BeFalse();

            long @long;
            r.TryReadInt64(out @long).Should().BeFalse();

            ulong @ulong;
            r.TryReadUInt64(out @ulong).Should().BeFalse();

            string lengthString;
            r.TryReadLengthString(out lengthString).Should().BeFalse();

            string padString;
            r.TryReadPaddedString(padLength, out padString).Should().BeFalse();

            byte[] bytes;
            r.TryRead(byteCount, out bytes).Should().BeFalse();
        }

        [Test]
        public void TryRead_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader
                .Invoking(
                    r =>
                    {
                        byte[] bytes;
                        r.TryRead(-1, out bytes);
                    })
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TrySkip_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader
                .Invoking(r => r.TrySkip(-1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TrySkipTo_Should_Throw_When_Offset_Is_Before_Current_Position()
        {
            var buffer = Helpers.GetRandomBytes(10);
            var reader = new PacketReader(buffer);

            reader.Skip(5);

            reader
                .Invoking(r => r.TrySkipTo(1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TrySkipTo_Should_Throw_When_Offset_Is_Negative()
        {
            var buffer = Helpers.GetRandomBytes(10);
            var reader = new PacketReader(buffer);

            reader
                .Invoking(r => r.TrySkipTo(-1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TrySkipTo_Should_Throw_When_Offset_Is_Before_Current_Position_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(20);
            var reader = new PacketReader(buffer, 10, 10);

            reader.Skip(5);

            reader
                .Invoking(r => r.TrySkipTo(1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TrySkipTo_Should_Throw_When_Offset_Is_Negative_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(20);
            var reader = new PacketReader(buffer, 10, 10);

            reader
                .Invoking(r => r.TrySkipTo(-1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void TryReadPaddedString_Should_Throw_When_PadLength_Is_Non_Positive(int padding)
        {
            EmptyReader
                .Invoking(
                    r =>
                    {
                        string result;
                        r.TryReadPaddedString(padding, out result);
                    })
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void TryReadPaddedString_Should_Return_False_When_Data_Is_Missing()
        {
            var buffer = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, };
            var reader = new PacketReader(buffer);

            string result;
            reader.TryReadPaddedString(13, out result).Should().BeFalse();
        }

        [Test]
        public void TryReadLengthString_Should_Return_False_When_Data_Is_Missing()
        {
            var buffer = new byte[] { 12, 0 }; // Length of 12.
            var reader = new PacketReader(buffer);

            string result;
            reader.TryReadLengthString(out result).Should().BeFalse();
        }

        [Test]
        public void Safe_Handling_Should_Throw_When_Reading_Callback_Is_Null_1()
        {
            EmptyReader
                .Invoking(r => r.Safe(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Safe_Handling_Should_Throw_When_Reading_Callback_Is_Null_2()
        {
            EmptyReader
                .Invoking(r => r.Safe(null, () => { }))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Safe_Handling_Should_Throw_When_Reading_Callback_Is_Null_3()
        {
            EmptyReader
                .Invoking(r => r.Safe(null, () => 1))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Safe_Handling_Should_Throw_When_Failure_Callback_Is_Null_1()
        {
            EmptyReader
                .Invoking(r1 => r1.Safe(r2 => { }, null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Safe_Handling_Should_Throw_When_Failure_Callback_Is_Null_2()
        {
            EmptyReader
                .Invoking(r1 => r1.Safe(r2 => 1, null))
                .ShouldThrow<ArgumentNullException>();
        }

        #endregion

        #region Does Not Throw

        [Test]
        public void TryReadBoolean_Does_Not_Move_Position_On_Failure()
        {
            bool result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadBoolean(out result));
        }

        [Test]
        public void TryReadByte_Does_Not_Move_Position_On_Failure()
        {
            byte result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadByte(out result));
        }

        [Test]
        public void TryReadInt16_Does_Not_Move_Position_On_Failure()
        {
            short result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadInt16(out result));
        }

        [Test]
        public void TryReadUInt16_Does_Not_Move_Position_On_Failure()
        {
            ushort result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadUInt16(out result));
        }

        [Test]
        public void TryReadInt32_Does_Not_Move_Position_On_Failure()
        {
            int result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadInt32(out result));
        }

        [Test]
        public void TryReadUInt32_Does_Not_Move_Position_On_Failure()
        {
            uint result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadUInt32(out result));
        }

        [Test]
        public void TryReadInt64_Does_Not_Move_Position_On_Failure()
        {
            long result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadInt64(out result));
        }

        [Test]
        public void TryReadUInt64_Does_Not_Move_Position_On_Failure()
        {
            ulong result;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryReadUInt64(out result));
        }

        [Test]
        public void TryRead_Does_Not_Move_Position_On_Failure()
        {
            byte[] bytes;
            ReturnsFalseAndDoesNotMovePosition(EmptyReader, r => r.TryRead(2, out bytes));
        }

        [Test]
        public void TryReadLengthString_Does_Not_Move_Position_On_Failure()
        {
            var buffer = new byte[] { 1, 0 }; // Length of 1.
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalseAndDoesNotMovePosition(reader, r => r.TryReadLengthString(out result));
        }

        [Test]
        public void TryReadPaddedString_Does_Not_Move_Position_On_Failure()
        {
            var buffer = new byte[] { 1, 2, 3, };
            var reader = new PacketReader(buffer);

            string result;
            ReturnsFalseAndDoesNotMovePosition(reader, r => r.TryReadPaddedString(buffer.Length + 1, out result));
        }

        private static void ReturnsFalseAndDoesNotMovePosition(PacketReader reader, Func<PacketReader, bool> action)
        {
            int oldRemaining = reader.Remaining;
            action(reader).Should().BeFalse();
            reader.Remaining.Should().Be(oldRemaining);
        }

        [Test]
        public void TryReadPaddedString_Should_Return_String_Without_Padding()
        {
            const string TestString = "shoftee";
            string paddedString = TestString.PadRight(13, '\0');

            var buffer = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(buffer);

            string result;
            reader.TryReadPaddedString(13, out result);
            result.Should().Be(TestString);
        }

        [Test]
        public void TryReadPaddedString_Should_Return_Strings_Shorter_Than_Padding()
        {
            var buffer = Encoding.UTF8.GetBytes("shoftee_shoftee_shoftee");
            var reader = new PacketReader(buffer);

            string result;
            reader.TryReadPaddedString(13, out result);
            result.Length.Should().BeLessThan(13);
        }

        [Test]
        public void TryReadLengthString_Should_Return_Same_String()
        {
            var reader = new PacketReader(new byte[] { 2, 0, 48, 49 });

            string result;
            reader.TryReadLengthString(out result).Should().BeTrue();

            result.Should().Be("01");
        }

        [Test]
        public void TryRead_Should_Return_Same_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            byte[] bytes;
            reader.TryRead(100, out bytes).Should().BeTrue();
            bytes.Should().ContainInOrder(buffer);
        }

        [Test]
        public void TryReadByte_Should_Return_Same_Bytes()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                byte result;
                reader.TryReadByte(out result).Should().BeTrue();
                result.Should().Be(b);
            }
        }

        [Test]
        public void TryReadBoolean_Should_Return_Correct_Values()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                bool result;
                reader.TryReadBoolean(out result).Should().BeTrue();
                result.Should().Be(b != 0);
            }
        }

        [Test]
        public void TryReadInt16_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 15, 255, };
            var reader = new PacketReader(buffer);

            int expected = (buffer[0]) + (buffer[1] << 8);

            short actual;
            reader.TryReadInt16(out actual).Should().BeTrue();
            actual.Should().Be((short)expected);
        }

        [Test]
        public void TryReadUInt16_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 15, 200, };
            var reader = new PacketReader(buffer);

            int expected = (buffer[0]) + (buffer[1] << 8);

            ushort actual;
            reader.TryReadUInt16(out actual).Should().BeTrue();
            actual.Should().Be((ushort)expected);
        }

        [Test]
        public void TryReadInt32_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 23, 23, 123, };
            var reader = new PacketReader(buffer);

            int expected = (buffer[0]) + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24);

            int actual;
            reader.TryReadInt32(out actual).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Test]
        public void TryReadUInt32_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 23, 234, };
            var reader = new PacketReader(buffer);

            uint expected = buffer[0] + ((uint)buffer[1] << 8) + ((uint)buffer[2] << 16) + ((uint)buffer[3] << 24);

            uint actual;
            reader.TryReadUInt32(out actual).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Test]
        public void TryReadInt64_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            long expected = buffer[0] + ((long)buffer[1] << 8) + ((long)buffer[2] << 16) + ((long)buffer[3] << 24) + ((long)buffer[4] << 32) + ((long)buffer[5] << 40) + ((long)buffer[6] << 48) + ((long)buffer[7] << 56);

            long actual;
            reader.TryReadInt64(out actual).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Test]
        public void TryReadUInt64_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            ulong expected = buffer[0] + ((ulong)buffer[1] << 8) + ((ulong)buffer[2] << 16) + ((ulong)buffer[3] << 24) + ((ulong)buffer[4] << 32) + ((ulong)buffer[5] << 40) + ((ulong)buffer[6] << 48) + ((ulong)buffer[7] << 56);

            ulong actual;
            reader.TryReadUInt64(out actual).Should().BeTrue();
            actual.Should().Be(expected);
        }

        [Test]
        public void TrySkip_Should_Skip_Correct_Number_Of_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            reader.TrySkip(70).Should().BeTrue();

            reader.Remaining.Should().Be(30);
        }

        [Test]
        public void TrySkipTo_Should_Skip_To_Correct_Offset()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            reader.TrySkipTo(10).Should().BeTrue();

            reader.Remaining.Should().Be(90);
        }

        [Test]
        public void Safe_Handling_Should_Call_ReadingCallback()
        {
            bool called = false;

            EmptyReader.Safe(r => called = true);

            called.Should().BeTrue();
        }

        [Test]
        public void Safe_Handling_Should_Call_FailureCallback()
        {
            bool called = false;

            EmptyReader.Safe(r => r.Skip(1), () => called = true);

            called.Should().BeTrue();
        }

        [Test]
        public void Safe_Handling_Should_Return_True_On_Success_1()
        {
            EmptyReader.Safe(r => { }).Should().BeTrue();
        }

        [Test]
        public void Safe_Handling_Should_Return_True_On_Success_2()
        {
            EmptyReader.Safe(r => { }, () => { }).Should().BeTrue();
        }

        [Test]
        public void Safe_Handling_Should_Return_False_On_Failure()
        {
            EmptyReader.Safe(r => r.Skip(1)).Should().BeFalse();
        }

        [Test]
        public void Safe_Handling_Should_Return_Value_On_Success()
        {
            const int Success = 0;
            const int Failure = 1;

            EmptyReader.Safe(r => Success, () => Failure).Should().Be(Success);
        }

        [Test]
        public void Safe_Handling_Should_Return_Value_On_Failure()
        {
            const int Failure = 1;

            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            reader.Safe(FailingReturningRead, () => Failure).Should().Be(Failure);
        }

        [Test]
        public void Safe_Handling_Should_Reset_Position_On_Failure_1()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            int expected = reader.Remaining;

            reader.Safe(FailingRead).Should().BeFalse();

            reader.Remaining.Should().Be(expected);
        }

        [Test]
        public void Safe_Handling_Should_Reset_Position_On_Failure_2()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            int expected = reader.Remaining;

            reader.Safe(FailingRead, () => { }).Should().BeFalse();

            reader.Remaining.Should().Be(expected);
        }

        [Test]
        public void Safe_Handling_Should_Reset_Position_On_Failure_3()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            const int Failure = 2;
            int expected = reader.Remaining;
            reader.Safe(FailingReturningRead, () => Failure).Should().Be(Failure);

            reader.Remaining.Should().Be(expected);
        }

        private static void FailingRead(IUnsafePacketReader reader)
        {
            reader.Skip(reader.Remaining - 1);
            reader.ReadInt32();
        }

        private static int FailingReturningRead(IUnsafePacketReader reader)
        {
            reader.Skip(reader.Remaining - 1);
            return reader.ReadInt32();
        }

        #endregion
    }
}