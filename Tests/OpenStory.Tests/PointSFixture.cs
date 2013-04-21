using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.Game;
using CommonExceptions = OpenStory.Exceptions;

namespace OpenStory.Tests
{
    [TestFixture]
    [Category("OpenStory.Common.Game.PointS")]
    public sealed class PointSFixture
    {
        [Test]
        public void MaxComponents_Should_Return_Correct_PointS()
        {
            var a = new PointS(10, 20);
            var b = new PointS(-20, 30);

            var c = PointS.MaxComponents(a, b);

            c.X.Should().Be(10);
            c.Y.Should().Be(30);
        }

        [Test]
        public void MinComponents_Should_Return_Correct_PointS()
        {
            var a = new PointS(-20, 30);
            var b = new PointS(20, -40);

            var c = PointS.MinComponents(a, b);

            c.X.Should().Be(-20);
            c.Y.Should().Be(-40);
        }

        [Test]
        public void Binary_Plus_Operator_Should_Return_Correct_PointS()
        {
            var a = new PointS(-20, 20);
            var b = new PointS(20, -20);
            var c = a + b;

            c.X.Should().Be(0);
            c.Y.Should().Be(0);
        }

        [Test]
        public void Unary_Minus_Operator_Should_Return_Correct_PointS()
        {
            var a = new PointS(20, -20);

            var b = -a;

            b.X.Should().Be(-20);
            b.Y.Should().Be(20);
        }

        [Test]
        public void Unary_Minus_Operator_Should_Throw_On_MinValue()
        {
            var point = new PointS(Int16.MinValue, Int16.MinValue);

            point.Invoking(a => { var b = -a; })
                 .ShouldThrow<ArgumentException>();
        }
    }
}
