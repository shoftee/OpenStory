using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common;

namespace OpenStory.Tests.Common
{
    [TestFixture]
    public sealed class PacketValueAttributeFixture
    {
        private enum TestEnum
        {
            [PacketValue(0x1234)]
            Defined,

            DefinedButNotDecorated,
        }

        [Category("OpenStory.Common.PacketValueAttribute")]
        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Defined()
        {
            const TestEnum Value = (TestEnum)20;
            Value.Invoking(v => v.ToPacketValue()).ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.PacketValueAttribute")]
        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Decorated()
        {
            const TestEnum Value = TestEnum.DefinedButNotDecorated;
            Value.Invoking(v => v.ToPacketValue()).ShouldThrow<ArgumentException>();
        }

        [Category("OpenStory.Common.PacketValueAttribute")]
        [Test]
        public void ToPacketValue_Should_Return_Decorating_Value()
        {
            TestEnum.Defined.ToPacketValue().Should().Be(0x1234);
        }
    }
}
