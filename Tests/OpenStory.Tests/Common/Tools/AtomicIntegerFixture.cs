using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.AtomicInteger")]
    [TestFixture]
    public sealed class AtomicIntegerFixture
    {
        [Test]
        public void Constructor_Should_Set_Value()
        {
            var i = new AtomicInteger(123);

            i.Value.Should().Be(123);
        }

        [Test]
        public void CompareExchange_Should_Set_New_Value_When_Comparand_Matches()
        {
            var i = new AtomicInteger(123);

            i.CompareExchange(123, 321);

            i.Value.Should().Be(321);
        }

        [Test]
        public void CompareExchange_Should_Return_Old_Value_When_Comparand_Matches()
        {
            var i = new AtomicInteger(123);

            var old = i.CompareExchange(123, 321);

            old.Should().Be(123);
        }

        [Test]
        public void CompareExchange_Should_Not_Set_New_Value_When_Comparand_Does_Not_Match()
        {
            var i = new AtomicInteger(123);

            i.CompareExchange(234, 432);

            i.Value.Should().Be(123);
        }

        [Test]
        public void CompareExchange_Should_Return_Current_Value_When_Comparand_Does_Not_Match()
        {
            var i = new AtomicInteger(123);

            var current = i.CompareExchange(234, 432);

            current.Should().Be(123);
        }

        [Test]
        public void Increment_Should_Set_Increased_Value()
        {
            var i = new AtomicInteger(123);

            i.Increment();

            i.Value.Should().Be(124);
        }

        [Test]
        public void Increment_Should_Return_Increased_Value()
        {
            var i = new AtomicInteger(123);

            var current = i.Increment();

            current.Should().Be(124);
        }

        [Test]
        public void Decrement_Should_Set_Decreased_Value()
        {
            var i = new AtomicInteger(123);

            i.Decrement();

            i.Value.Should().Be(122);
        }

        [Test]
        public void Decrement_Should_Return_Decreased_Value()
        {
            var i = new AtomicInteger(123);

            var current = i.Decrement();

            current.Should().Be(122);
        }

        [Test]
        public void ExchangeWith_Should_Set_New_Value()
        {
            var i = new AtomicInteger(123);

            i.ExchangeWith(321);

            i.Value.Should().Be(321);
        }

        [Test]
        public void ExchangeWith_Should_Return_Old_Value()
        {
            var i = new AtomicInteger(123);

            var old = i.ExchangeWith(321);

            old.Should().Be(123);
        }

        [Test]
        public void ToInt32_Should_Return_Value_As_Int32()
        {
            var i = new AtomicInteger(123);

            i.ToInt32().Should().Be(123);
        }

        [Test]
        public void Static_ToInt32_Should_Return_Value_As_Int32()
        {
            var i = new AtomicInteger(123);

            AtomicInteger.ToInt32(i).Should().Be(123);
        }

        [Test]
        public void Static_ToInt32_Should_Throw_On_Null()
        {
            Action action = () => AtomicInteger.ToInt32(null);

            action.ShouldThrow<InvalidCastException>();
        }

        [Test]
        public void Implicit_Cast_From_Int32_Should_Set_Value()
        {
            AtomicInteger i = 123;
            i.Value.Should().Be(123);
        }

        [Test]
        public void Explicit_Cast_From_Int32_Should_Set_Value()
        {
            var i = (AtomicInteger)123;
            i.Value.Should().Be(123);
        }

        [Test]
        public void Explicit_Cast_From_Int32_Should_Throw_On_Null()
        {
            Action action = () => ((int)(AtomicInteger)null).Whatever();

            action.ShouldThrow<InvalidCastException>();
        }

        [Test]
        public void Explicit_Cast_To_Int32_Should_Return_Value()
        {
            var i = new AtomicInteger(123);
            var cast = (int)i;
            cast.Should().Be(123);
        }
    }
}
