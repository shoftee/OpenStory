using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common.IO
{
    [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
    [TestFixture]
    internal sealed class UnsafePacketReadingFixture : PacketReaderFixtureBase
    {
        #region Throws

        [Test]
        public void Reading_Should_Throw_In_Zero_Length_Segment()
        {
            ThrowsPreOnReadOperations(EmptyReader, 1, 13, 10);
        }

        [Test]
        public void Reading_Should_Throw_In_Offset_Zero_Length_Segment()
        {
            var buffer = new byte[] { 1 };
            var reader = new PacketReader(buffer, buffer.Length, 0);

            ThrowsPreOnReadOperations(reader, 1, 13, 10);
        }

        private static void ThrowsPreOnReadOperations(PacketReader reader, int skip, int padLength, int byteCount)
        {
            reader
                .Invoking(r => r.Skip(skip))
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadBoolean())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadByte())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadInt16())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadUInt16())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadInt32())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadUInt32())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadInt64())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadUInt64())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadLengthString())
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadPaddedString(padLength))
                .ShouldThrow<PacketReadingException>();

            reader
                .Invoking(r => r.ReadBytes(byteCount))
                .ShouldThrow<PacketReadingException>();
        }

        [Test]
        public void ReadBytes_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader
                .Invoking(r => r.ReadBytes(-1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Skip_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader
                .Invoking(r => r.Skip(-1))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ReadPaddedString_Should_Throw_When_Padding_Is_Non_Positive(int padding)
        {
            EmptyReader
                .Invoking(r => r.ReadPaddedString(padding))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ReadPaddedString_Should_Throw_When_Data_Is_Missing()
        {
            var reader = new PacketReader(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1 });
            reader
                .Invoking(r => r.ReadPaddedString(13))
                .ShouldThrow<PacketReadingException>();
        }

        [Test]
        public void ReadLengthString_Should_Throw_When_Data_Is_Missing()
        {
            var reader = new PacketReader(new byte[] { 12, 0 });
            reader
                .Invoking(r => r.ReadLengthString())
                .ShouldThrow<PacketReadingException>();
        }

        #endregion

        #region Does Not Throw

        [Test]
        public void ReadBoolean_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadBoolean());
        }

        [Test]
        public void Skip_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.Skip(1));
        }

        [Test]
        public void ReadByte_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadByte());
        }

        [Test]
        public void ReadInt16_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadInt16());
        }

        [Test]
        public void ReadUInt16_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt16());
        }

        [Test]
        public void ReadInt32_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadInt32());
        }

        [Test]
        public void ReadUInt32_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt32());
        }

        [Test]
        public void ReadInt64_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadInt64());
        }

        [Test]
        public void ReadUInt64_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt64());
        }

        [Test]
        public void ReadBytes_Should_Not_Move_Position_On_Failure()
        {
            ThrowsAndDoesNotMovePosition(EmptyReader, r => r.ReadBytes(2));
        }

        [Test]
        public void ReadLengthString_Should_Not_Move_Position_On_Failure()
        {
            var reader = new PacketReader(new byte[] { 1, 0 });

            ThrowsAndDoesNotMovePosition(reader, r => r.ReadLengthString());
        }

        [Test]
        public void ReadPaddedString_Should_Not_Move_Position_On_Failure()
        {
            var reader = new PacketReader(new byte[] { 1, 2, 3 });

            ThrowsAndDoesNotMovePosition(reader, r => r.ReadPaddedString(4));
        }

        [Test]
        public void ReadPaddedString_Should_Return_String_Without_Padding()
        {
            string paddedString = "shoftee".PadRight(13, '\0');

            var buffer = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(buffer);

            var actual = reader.ReadPaddedString(13);

            actual.Should().Be("shoftee");
        }

        [Test]
        public void ReadPaddedString_Should_Return_String_Shorter_Than_Padding()
        {
            const int Padding = 13;
            var buffer = Encoding.UTF8.GetBytes("shoftee_shoftee_shoftee");

            var reader = new PacketReader(buffer);
            int actual = reader.ReadPaddedString(Padding).Length;

            actual.Should().BeLessThan(Padding);
        }

        [Test]
        public void ReadLengthString_Should_Return_Same_String()
        {
            // (length 2) "01"
            var reader = new PacketReader(new byte[] { 0x02, 0x00, 0x30, 0x31 });

            string actual = reader.ReadLengthString();

            actual.Should().Be("01");
        }

        [Test]
        public void ReadBytes_Should_Read_Correct_Count_Of_Bytes()
        {
            const int BufferSize = 100;
            const int ReadCount = 50;

            var buffer = Helpers.GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            byte[] bytes = reader.ReadBytes(ReadCount);

            bytes.Should().ContainInOrder(buffer.CopySegment(0, ReadCount));
        }

        [Test]
        public void ReadFully_Should_Read_All_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            byte[] bytes = reader.ReadFully();

            bytes.Should().ContainInOrder(buffer);
        }

        [Test]
        public void ReadFully_Should_Read_All_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer, 10, 80);

            var expected = buffer.CopySegment(10, 80);

            byte[] actual = reader.ReadFully();

            actual.Should().ContainInOrder(expected);
        }

        [Test]
        public void ReadFully_Should_Read_All_Remaining_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var expected = buffer.CopySegment(10, 90);

            var reader = new PacketReader(buffer);
            reader.Skip(10);
            byte[] actual = reader.ReadFully();

            actual.Should().ContainInOrder(expected);
        }

        [Test]
        public void ReadFully_Should_Read_All_Remaining_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var expected = buffer.CopySegment(20, 70);

            var reader = new PacketReader(buffer, 10, 80);
            reader.Skip(10);
            byte[] actual = reader.ReadFully();

            actual.Should().ContainInOrder(expected);
        }

        [Test]
        public void ReadByte_Should_Read_Bytes_Correctly()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4 };
            var reader = new PacketReader(buffer);

            foreach (var expected in buffer)
            {
                byte actual = reader.ReadByte();

                actual.Should().Be(expected);
            }
        }

        [Test]
        public void ReadBoolean_Should_Read_Correct_Values()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4 };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                bool expected = b != 0;

                bool actual = reader.ReadBoolean();

                actual.Should().Be(expected);
            }
        }

        [Test]
        public void ReadInt16_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(2);
            int expected = (buffer[0]) + (buffer[1] << 8);

            var reader = new PacketReader(buffer);
            short actual = reader.ReadInt16();
            
            actual.Should().Be((short)expected);
        }

        [Test]
        public void ReadUInt16_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(2);
            int expected = (buffer[0]) + (buffer[1] << 8);

            var reader = new PacketReader(buffer);
            ushort actual = reader.ReadUInt16();

            actual.Should().Be((ushort)expected);
        }

        [Test]
        public void ReadInt32_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(4);
            int expected = (buffer[0]) + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24);

            var reader = new PacketReader(buffer);
            int actual = reader.ReadInt32();

            actual.Should().Be(expected);
        }

        [Test]
        public void ReadUInt32_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(4);
            uint expected = (uint)(buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));

            var reader = new PacketReader(buffer);
            uint actual = reader.ReadUInt32();

            actual.Should().Be(expected);
        }

        [Test]
        public void ReadInt64_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(8);
            long expected = buffer[0] + ((long)buffer[1] << 8) + ((long)buffer[2] << 16) + ((long)buffer[3] << 24) + ((long)buffer[4] << 32) + ((long)buffer[5] << 40) + ((long)buffer[6] << 48) + ((long)buffer[7] << 56);

            var reader = new PacketReader(buffer);
            long actual = reader.ReadInt64();

            actual.Should().Be(expected);
        }

        [Test]
        public void ReadUInt64_Should_Read_In_LittleEndian()
        {
            var buffer = Helpers.GetRandomBytes(8);
            ulong expected = buffer[0] + ((ulong)buffer[1] << 8) + ((ulong)buffer[2] << 16) + ((ulong)buffer[3] << 24) + ((ulong)buffer[4] << 32) + ((ulong)buffer[5] << 40) + ((ulong)buffer[6] << 48) + ((ulong)buffer[7] << 56);

            var reader = new PacketReader(buffer);
            ulong actual = reader.ReadUInt64();

            actual.Should().Be(expected);
        }

        [Test]
        public void Skip_Should_Skip_Correct_Number_Of_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);
            const int ExpectedSkip = 10;

            int oldRemaining = reader.Remaining;
            reader.Skip(ExpectedSkip);
            int newRemaining = reader.Remaining;

            int actualSkip = oldRemaining - newRemaining;
            actualSkip.Should().Be(ExpectedSkip);
        }

        #endregion

        private static void ThrowsAndDoesNotMovePosition(PacketReader reader, Action<PacketReader> action)
        {
            int expected = reader.Remaining;
            reader.Invoking(action).ShouldThrow<PacketReadingException>();

            int actual = reader.Remaining;
            actual.Should().Be(expected);
        }

        private static PacketReader EmptyReader
        {
            get { return new PacketReader(Helpers.EmptyBuffer); }
        }
    }
}
