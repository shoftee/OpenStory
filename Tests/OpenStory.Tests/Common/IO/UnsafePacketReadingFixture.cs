using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.Tools;
using OpenStory.Tests;

namespace OpenStory.Common.IO
{
    [TestFixture]
    internal sealed class UnsafePacketReadingFixture
    {
        #region Throws

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void Reading_Should_Throw_In_Zero_Length_Segment()
        {
            ThrowsPreOnReadOperations(EmptyReader, 1, 13, 10);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void Reading_Should_Throw_In_Offset_Zero_Length_Segment()
        {
            var buffer = new byte[] { 1, };
            var reader = new PacketReader(buffer, buffer.Length, 0);

            ThrowsPreOnReadOperations(reader, 1, 13, 10);
        }

        private static void ThrowsPreOnReadOperations(PacketReader reader, int skip, int padLength, int byteCount)
        {
            reader.Invoking(r => r.Skip(skip)).ShouldThrow<PacketReadingException>();

            reader.Invoking(r => r.ReadBoolean()).ShouldThrow<PacketReadingException>();

            reader.Invoking(r => r.ReadByte()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadInt16()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadUInt16()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadInt32()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadUInt32()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadInt64()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadUInt64()).ShouldThrow<PacketReadingException>();

            reader.Invoking(r => r.ReadLengthString()).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadPaddedString(padLength)).ShouldThrow<PacketReadingException>();
            reader.Invoking(r => r.ReadBytes(byteCount)).ShouldThrow<PacketReadingException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadBytes_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader.Invoking(r => r.ReadBytes(-1))
                       .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void Skip_Should_Throw_When_Count_Is_Negative()
        {
            EmptyReader.Invoking(r => r.Skip(-1))
                       .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void ReadPaddedString_Should_Throw_When_Padding_Is_Non_Positive(int padding)
        {
            EmptyReader.Invoking(r => r.ReadPaddedString(padding))
                       .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadPaddedString_Should_Throw_When_Data_Is_Missing()
        {
            var reader = new PacketReader(new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, });
            reader.Invoking(r => r.ReadPaddedString(13)).ShouldThrow<PacketReadingException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadLengthString_Should_Throw_When_Data_Is_Missing()
        {
            var reader = new PacketReader(new byte[] { 12, 0 });
            reader.Invoking(r => r.ReadLengthString()).ShouldThrow<PacketReadingException>();
        }

        #endregion

        #region Does Not Throw

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadBoolean_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadBoolean());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void Skip_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.Skip(1));
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadByte_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadByte());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt16_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadInt16());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt16_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt16());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt32_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadInt32());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt32_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt32());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt64_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadInt64());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt64_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadUInt64());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadBytes_Should_Not_Move_Position_On_Failure()
        {
            ThrowsPreAndDoesNotMovePosition(EmptyReader, r => r.ReadBytes(2));
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadLengthString_Should_Not_Move_Position_On_Failure()
        {
            var reader = new PacketReader(new byte[] { 1, 0 });

            ThrowsPreAndDoesNotMovePosition(reader, r => r.ReadLengthString());
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadPaddedString_Should_Not_Move_Position_On_Failure()
        {
            var reader = new PacketReader(new byte[] { 1, 2, 3, });

            ThrowsPreAndDoesNotMovePosition(reader, r => r.ReadPaddedString(4));
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadPaddedString_Should_Return_String_Without_Padding()
        {
            string paddedString = "shoftee".PadRight(13, '\0');

            var buffer = Encoding.UTF8.GetBytes(paddedString);
            var reader = new PacketReader(buffer);

            var actual = reader.ReadPaddedString(13);

            actual.Should().Be("shoftee");
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadPaddedString_Should_Return_String_Shorter_Than_Padding()
        {
            var buffer = Encoding.UTF8.GetBytes("shoftee_shoftee_shoftee");
            var reader = new PacketReader(buffer);

            reader.ReadPaddedString(13).Length.Should().BeLessThan(13);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadLengthString_Should_Return_Same_String()
        {
            var reader = new PacketReader(new byte[] { 2, 0, 48, 49 });

            reader.ReadLengthString().Should().Be("01");
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadBytes_Should_Read_Correct_Count_Of_Bytes()
        {
            const int BufferSize = 100;
            const int ReadCount = 50;

            var buffer = Helpers.GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            reader.ReadBytes(ReadCount).Should().ContainInOrder(buffer.CopySegment(0, ReadCount));
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadFully_Should_Read_All_Bytes()
        {
            const int BufferSize = 100;

            var buffer = Helpers.GetRandomBytes(BufferSize);
            var reader = new PacketReader(buffer);

            reader.ReadFully().Should().ContainInOrder(buffer);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadFully_Should_Read_All_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer, 10, 80);

            var expected = buffer.CopySegment(10, 80);

            reader.ReadFully().Should().ContainInOrder(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadFully_Should_Read_All_Remaining_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);
            reader.Skip(10);

            var expected = buffer.CopySegment(10, 90);

            reader.ReadFully().Should().ContainInOrder(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadFully_Should_Read_All_Remaining_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer, 10, 80);

            reader.Skip(10);

            var expected = buffer.CopySegment(20, 70);

            reader.ReadFully().Should().ContainInOrder(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadByte_Should_Read_Read_Bytes_Correctly()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                reader.ReadByte().Should().Be(b);
            }
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadBoolean_Should_Read_Correct_Values()
        {
            var buffer = new byte[] { 0, 1, 0, 2, 0, 3, 0, 4, };
            var reader = new PacketReader(buffer);

            foreach (var b in buffer)
            {
                reader.ReadBoolean().Should().Be(b != 0);
            }
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt16_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 15, 100, };
            var reader = new PacketReader(buffer);

            short actual = reader.ReadInt16();
            int expected = (buffer[0]) + (buffer[1] << 8);
            actual.Should().Be((short)expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt16_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 15, 200, };
            var reader = new PacketReader(buffer);

            ushort actual = reader.ReadUInt16();
            int expected = (buffer[0]) + (buffer[1] << 8);
            actual.Should().Be((ushort)expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt32_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 23, 23, 123, };
            var reader = new PacketReader(buffer);

            int actual = reader.ReadInt32();
            int expected = (buffer[0]) + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24);

            actual.Should().Be(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt32_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 23, 234, };
            var reader = new PacketReader(buffer);

            uint actual = reader.ReadUInt32();
            uint expected = (uint) (buffer[0] + (buffer[1] << 8) + (buffer[2] << 16) + (buffer[3] << 24));

            actual.Should().Be(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadInt64_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            long actual = reader.ReadInt64();
            long expected = buffer[0] + ((long)buffer[1] << 8) + ((long)buffer[2] << 16) + ((long)buffer[3] << 24) + ((long)buffer[4] << 32) + ((long)buffer[5] << 40) + ((long)buffer[6] << 48) + ((long)buffer[7] << 56);

            actual.Should().Be(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void ReadUInt64_Should_Read_In_LittleEndian()
        {
            var buffer = new byte[] { 123, 234, 123, 234, 123, 23, 34, 255, };
            var reader = new PacketReader(buffer);

            ulong actual = reader.ReadUInt64();
            ulong expected = buffer[0] + ((ulong)buffer[1] << 8) + ((ulong)buffer[2] << 16) + ((ulong)buffer[3] << 24) + ((ulong)buffer[4] << 32) + ((ulong)buffer[5] << 40) + ((ulong)buffer[6] << 48) + ((ulong)buffer[7] << 56);

            actual.Should().Be(expected);
        }

        [Category("OpenStory.Common.IO.PacketReader.Unsafe")]
        [Test]
        public void Skip_Should_Skip_Correct_Number_Of_Bytes()
        {
            var buffer = Helpers.GetRandomBytes(100);
            var reader = new PacketReader(buffer);

            int oldRemaining = reader.Remaining;
            reader.Skip(10);
            int newRemaining = reader.Remaining;

            int actualSkipLength = oldRemaining - newRemaining;
            actualSkipLength.Should().Be(10);
        }

        #endregion

        private static void ThrowsPreAndDoesNotMovePosition(PacketReader reader, Action<PacketReader> action)
        {
            int remainingOld = reader.Remaining;
            reader.Invoking(action).ShouldThrow<PacketReadingException>();
            reader.Remaining.Should().Be(remainingOld);
        }

        private static PacketReader EmptyReader
        {
            get { return new PacketReader(Helpers.Empty); }
        }
    }
}
