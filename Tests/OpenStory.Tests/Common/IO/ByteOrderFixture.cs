using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests.Common.IO
{
    [Category("OpenStory.Common.IO.ByteOrder")]
    [TestFixture]
    public class ByteOrderFixture
    {
        [Test]
        public void FlipBytes_Should_Flip_Int16()
        {
            short number = 0x1234;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(0x3412);
        }

        [Test]
        public void FlipBytes_Should_Flip_UInt16()
        {
            ushort number = 0xABCD;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(0xCDAB);
        }

        [Test]
        public void FlipBytes_Should_Flip_Int32()
        {
            int number = 0x12345678;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(0x78563412);
        }

        [Test]
        public void FlipBytes_Should_Flip_UInt32()
        {
            uint number = 0x890ABCDE;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(0xDEBC0A89);
        }

        [Test]
        public void FlipBytes_Should_Flip_Int64()
        {
            long number = 0x0123456789ABCDEF;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(unchecked((long)0xEFCDAB8967452301));
        }

        [Test]
        public void FlipBytes_Should_Flip_UInt64()
        {
            ulong number = 0x89ABCDEF01234567;

            ByteOrder.FlipBytes(ref number);

            number.Should().Be(0x67452301EFCDAB89);
        }
    }
}
