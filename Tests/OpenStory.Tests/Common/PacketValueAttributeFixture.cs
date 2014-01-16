using System;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.PacketValueAttribute")]
    [TestFixture]
    public sealed class PacketValueAttributeFixture
    {
        private enum TestEnum
        {
            [PacketValue(0x1234)]
            Defined,

            DefinedButNotDecorated,
        }

        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Defined()
        {
            const TestEnum Value = (TestEnum)20;
            Value
                .Invoking(v => v.ToPacketValue())
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Decorated()
        {
            const TestEnum Value = TestEnum.DefinedButNotDecorated;
            Value
                .Invoking(v => v.ToPacketValue())
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ToPacketValue_Should_Return_Decorating_Value()
        {
            TestEnum.Defined.ToPacketValue().Should().Be(0x1234);
        }
    }
}
