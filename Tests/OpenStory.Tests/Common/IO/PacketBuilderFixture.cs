using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.Game;
using OpenStory.Common.IO;

namespace OpenStory.Tests.Common.IO
{
    [TestFixture]
    [Category("OpenStory.Common.IO.PacketBuilder")]
    sealed class PacketBuilderFixture
    {
        private static PacketBuilder DefaultBuilder
        {
            get { return new PacketBuilder(); }
        }

        #region Failure

        [Test]
        public void WriteBoolean_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteBoolean(true));
        }

        [Test]
        public void WriteByte_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteByte(0x77));
        }

        [Test]
        public void WriteInt16_Should_Throw_After_Disposal_Signed()
        {
            ShouldThrowOde(b => b.WriteInt16(0x7777));
        }

        [Test]
        public void WriteInt16_Should_Throw_After_Disposal_Unsigned()
        {
            ShouldThrowOde(b => b.WriteInt16(0x8889));
        }

        [Test]
        public void WriteInt32_Should_Throw_After_Disposal_Signed()
        {
            ShouldThrowOde(b => b.WriteInt32(0x77777777));
        }

        [Test]
        public void WriteInt32_Should_Throw_After_Disposal_Unsigned()
        {
            ShouldThrowOde(b => b.WriteInt32(0x88888889));
        }

        [Test]
        public void WriteInt64_Should_Throw_After_Disposal_Signed()
        {
            ShouldThrowOde(b => b.WriteInt64(0x7777777777777777));
        }

        [Test]
        public void WriteInt64_Should_Throw_After_Disposal_Unsigned()
        {
            ShouldThrowOde(b => b.WriteInt64(0x8888888888888889));
        }

        [Test]
        public void WriteBytes_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteBytes(new byte[] { 123, 43, 32, 123 }));
        }

        [Test]
        public void WriteLengthString_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteLengthString("1234"));
        }

        [Test]
        public void WritePaddedString_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WritePaddedString("shoftee", 13));
        }

        [Test]
        public void WriteZeroes_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteZeroes(123));
        }

        [Test]
        public void ToByteArray_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.ToByteArray());
        }

        [Test]
        public void WriteVector_Should_Throw_After_Disposal()
        {
            ShouldThrowOde(b => b.WriteVector(new PointS(123, 321)));
        }

        private static void ShouldThrowOde(Action<PacketBuilder> operation)
        {
            var builder = new PacketBuilder();
            builder.Dispose();

            builder.Invoking(operation).ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        public void WriteZero_Should_Throw_On_Negative_Zero_Count()
        {
            DefaultBuilder.Invoking(b => b.WriteZeroes(-1))
                          .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void WritePaddedString_Should_Throw_On_Non_Positive_Padding(int padding)
        {
            DefaultBuilder.Invoking(b => b.WritePaddedString("123", padding))
                          .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void WritePaddedString_Should_Throw_On_Insufficient_Padding()
        {
            DefaultBuilder.Invoking(b => b.WritePaddedString("123", 3))
                          .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void WritePaddedString_Should_Throw_On_Null_String()
        {
            DefaultBuilder.Invoking(b => b.WritePaddedString(null, 13))
                          .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void WriteLengthString_Should_Throw_On_Null_String()
        {
            DefaultBuilder.Invoking(b => b.WriteLengthString(null))
                          .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Default_Constructor_Should_Throw_On_Null_Buffer()
        {
            DefaultBuilder.Invoking(b => b.WriteBytes(null))
                          .ShouldThrow<ArgumentNullException>();
        }

        #endregion

        #region Non-failure

        [Test]
        public void Double_Dispose_Should_Not_Throw()
        {
            var builder = new PacketBuilder();
            builder.Dispose();

            builder.Invoking(b => b.Dispose()).ShouldNotThrow();
        }

        [Test]
        public void Byte_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteByte(123);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 123 });
        }

        [Test]
        public void Int16_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt16(0x7987);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x87, 0x79, });
        }

        [Test]
        public void UInt16_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt16(0x8987);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x87, 0x89, });
        }

        [Test]
        public void Int32_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt32(0x79873412);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x12, 0x34, 0x87, 0x79, });
        }

        [Test]
        public void UInt32_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt32(0x89873412);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x12, 0x34, 0x87, 0x89, });
        }

        [Test]
        public void Int64_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt64(0x7987341278563412);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x87, 0x79, });
        }

        [Test]
        public void UInt64_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteInt64(0x8987341278563412);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x12, 0x34, 0x87, 0x89, });
        }

        [Test]
        public void True_And_False_Are_Written_As_Integers_Correctly()
        {
            var builder = new PacketBuilder();

            builder.WriteBoolean(true);
            builder.WriteBoolean(false);
            builder.WriteBoolean(true);
            builder.WriteBoolean(false);
            builder.WriteBoolean(true);
            builder.WriteBoolean(false);

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x1, 0x0, 0x1, 0x0, 0x1, 0x0 });
        }

        [Test]
        public void LengthString_Length_Should_Be_Written_In_LittleEndian()
        {
            var builder = new PacketBuilder();

            builder.WriteLengthString("01");

            builder.ToByteArray()
                   .Take(2)
                   .Should().ContainInOrder(new byte[] { 0x02, 0x00 });
        }

        [Test]
        public void LengthString_String_Should_Be_Written_In_Utf8()
        {
            var builder = new PacketBuilder();

            const string TestString = "01";
            builder.WriteLengthString(TestString);

            var stringBytes = Encoding.UTF8.GetBytes(TestString);
            builder.ToByteArray()
                   .Skip(2)
                   .Should().ContainInOrder(stringBytes);
        }

        [Test]
        public void PaddedString_Should_Be_Null_Terminated()
        {
            var builder = new PacketBuilder();

            const string TestString = "1234";
            const int Padding = 13;
            builder.WritePaddedString(TestString, Padding);

            builder.ToByteArray()
                   .Skip(TestString.Length).First()
                   .Should().Be(0);
        }

        [Test]
        public void PaddedString_Should_Be_Written_In_Utf8()
        {
            var builder = new PacketBuilder();

            const string TestString = "1234";
            builder.WritePaddedString(TestString, 13);

            var stringBytes = Encoding.UTF8.GetBytes(TestString);
            builder.ToByteArray()
                   .Take(TestString.Length)
                   .Should().ContainInOrder(stringBytes);
        }

        [Test]
        public void WriteZeroes_Should_Zeroes()
        {
            var builder = new PacketBuilder();

            builder.WriteByte(0x12);
            builder.WriteByte(0x34);
            builder.WriteZeroes(5);
            builder.WriteByte(0x56);

            builder.ToByteArray()
                   .Skip(2).Take(5)
                   .Should().OnlyContain(b => b == 0);
        }

        [Test]
        public void WriteBytes_Should_Write_Same_Bytes()
        {
            var builder = new PacketBuilder();

            builder.WriteBytes(new byte[] { 0x12, 0x34, 0x56, 0x78, });

            builder.ToByteArray()
                   .Should().ContainInOrder(new byte[] { 0x12, 0x34, 0x56, 0x78, });
        }

        #endregion
    }
}
