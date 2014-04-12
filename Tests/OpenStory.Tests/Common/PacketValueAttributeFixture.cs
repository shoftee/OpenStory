using System;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.PacketValueAttribute")]
    [TestFixture]
    public sealed class PacketValueAttributeFixture
    {
        [Test]
        public void ToPacketValue_Should_Return_Decorating_Value()
        {
            var packetValue = DefinedEnumValue.ToPacketValue();
            packetValue.Should().Be(DefinedPacketValue);
        }

        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Defined()
        {
            UndefinedEnumValue
                .Invoking(v => v.ToPacketValue())
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void ToPacketValue_Should_Throw_If_Member_Not_Decorated()
        {
            DefinedButNotDecorated
                .Invoking(v => v.ToPacketValue())
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void ToEnumValue_Should_Return_Decorated_Named_Value()
        {
            var enumValue = DefinedPacketValue.ToEnumValue<TestEnum>();
            enumValue.Should().Be(TestEnum.Defined);
        }

        [Test]
        public void ToEnumValue_Should_Throw_If_No_Decorated_Match_Exists()
        {
            1234
                .Invoking(v => v.ToEnumValue<TestEnum>())
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        #region Helpful things!

        private const int DefinedPacketValue = 0x1234;

        private enum TestEnum
        {
            [PacketValue(DefinedPacketValue)]
            Defined,

            DefinedButNotDecorated,
        }

        private static TestEnum DefinedEnumValue
        {
            get { return TestEnum.Defined; }
        }

        private static TestEnum UndefinedEnumValue
        {
            get { return (TestEnum)20; }
        }

        private static TestEnum DefinedButNotDecorated
        {
            get { return TestEnum.DefinedButNotDecorated; }
        }

        #endregion
    }
}
