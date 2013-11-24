using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.AtomicBoolean")]
    [TestFixture]
    public sealed class AtomicBooleanFixture
    {
        [Test]
        public void Constructor_Should_Set_Value_To_True()
        {
            var b = new AtomicBoolean(true);
            b.Value.Should().BeTrue();
        }

        [Test]
        public void Constructor_Should_Set_Value_To_False()
        {
            var b = new AtomicBoolean(false);
            b.Value.Should().BeFalse();
        }

        [Test]
        public void ToBoolean_Should_Return_True()
        {
            var b = new AtomicBoolean(true);
            b.ToBoolean().Should().BeTrue();
        }

        [Test]
        public void Static_ToBoolean_Should_Return_True()
        {
            var b = new AtomicBoolean(true);
            AtomicBoolean.ToBoolean(b).Should().BeTrue();
        }

        [Test]
        public void ToBoolean_Should_Return_False()
        {
            var b = new AtomicBoolean(false);
            b.ToBoolean().Should().BeFalse();
        }

        [Test]
        public void Static_ToBoolean_Should_Return_False()
        {
            var b = new AtomicBoolean(false);
            AtomicBoolean.ToBoolean(b).Should().BeFalse();
        }

        [Test]
        public void Static_ToBoolean_Should_Throw_On_Null()
        {
            Action action = () => AtomicBoolean.ToBoolean(null);

            action.ShouldThrow<InvalidCastException>();
        }

        [Test]
        public void Exchange_Should_Set_New_Value()
        {
            var b = new AtomicBoolean(false);

            b.Exchange(true);

            b.Value.Should().BeTrue();
        }

        [Test]
        public void Exchange_Should_Return_Old_Value()
        {
            var b = new AtomicBoolean(false);

            var old = b.Exchange(true);

            old.Should().BeFalse();
        }

        [Test]
        public void Explicit_Cast_Should_Cast_From_True()
        {
            var b = (AtomicBoolean)true;
            b.Value.Should().BeTrue();
        }

        [Test]
        public void Explicit_Cast_Should_Cast_From_False()
        {
            var b = (AtomicBoolean)false;
            b.Value.Should().BeFalse();
        }

        [Test]
        public void Implicit_Cast_Should_Cast_From_True()
        {
            AtomicBoolean b = true;
            b.Value.Should().BeTrue();
        }

        [Test]
        public void Implicit_Cast_Should_Cast_From_False()
        {
            AtomicBoolean b = false;
            b.Value.Should().BeFalse();
        }

        [Test]
        public void CompareExchange_Should_Set_New_Value_When_Comparand_Matches()
        {
            var b = new AtomicBoolean(true);

            b.CompareExchange(true, false);

            b.Value.Should().BeFalse();
        }

        [Test]
        public void CompareExchange_Should_Return_Old_Value_When_Comparand_Matches()
        {
            var b = new AtomicBoolean(true);

            var old = b.CompareExchange(true, false);

            old.Should().BeTrue();
        }

        [Test]
        public void CompareExchange_Should_Not_Set_New_Value_When_Comparand_Does_Not_Match()
        {
            var b = new AtomicBoolean(true);

            b.CompareExchange(false, false);

            b.Value.Should().BeTrue();
        }

        [Test]
        public void CompareExchange_Should_Return_Current_Value_When_Comparand_Does_Not_Match()
        {
            var b = new AtomicBoolean(true);

            var current = b.CompareExchange(false, false);

            current.Should().BeTrue();
        }

        [Test]
        public void Explicit_Cast_Should_Cast_To_True()
        {
            var b = new AtomicBoolean(true);
            var cast = (bool)b;
            cast.Should().BeTrue();
        }

        [Test]
        public void Explicit_Cast_Should_Cast_To_False()
        {
            var b = new AtomicBoolean(false);
            var cast = (bool)b;
            cast.Should().BeFalse();
        }

        [Test]
        public void Explicit_Cast_Should_Throw_On_Casting_Null()
        {
            Action action = () => ((bool)(AtomicBoolean)null).Whatever();

            action.ShouldThrow<InvalidCastException>();
        }
    }
}
