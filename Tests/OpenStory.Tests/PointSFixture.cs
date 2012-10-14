using System;
using NUnit.Framework;
using OpenStory.Common.Game;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.Game", Description = "PointS tests.")]
    public sealed class PointSFixture
    {
        [Test]
        public void MaxComponentsWorks()
        {
            var a = new PointS(10, 20);
            var b = new PointS(-20, 30);

            var c = PointS.MaxComponents(a, b);

            Assert.AreEqual(c.X, Math.Max(a.X, b.X));
            Assert.AreEqual(c.Y, Math.Max(a.Y, b.Y));
        }

        [Test]
        public void MinComponentsWorks()
        {
            var a = new PointS(-20, 30);
            var b = new PointS(20, -40);

            var c = PointS.MinComponents(a, b);

            Assert.AreEqual(c.X, Math.Min(a.X, b.X));
            Assert.AreEqual(c.Y, Math.Min(a.Y, b.Y));
        }

        [Test]
        public void PointAdditionWorks()
        {
            var a = new PointS(-20, 20);
            var b = new PointS(20, -20);
            var c = a + b;

            Assert.AreEqual(c.X, a.X + b.X);
            Assert.AreEqual(c.Y, a.Y + b.Y);
        }

        [Test]
        public void PointNegationWorks()
        {
            var a = new PointS(20, -20);

            var b = -a;

            Assert.AreEqual(a.X, -b.X);
            Assert.AreEqual(a.Y, -b.Y);
        }

        [Test]
        public void PointNegationThrowsOnMinValue()
        {
            var a = new PointS(Int16.MinValue, Int16.MinValue);

            Assert.Throws<ArgumentException>(() => { var b = -a; });
            Assert.Throws<ArgumentException>(() => { var b = -a; });
        }
    }
}
