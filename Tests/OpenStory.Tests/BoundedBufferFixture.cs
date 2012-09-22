using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.IO", Description = "BoundedBuffer tests.")]
    sealed class BoundedBufferFixture
    {
        [Test]
        public void ThrowsOnNegativeCapacity()
        {
            Helpers.ThrowsAoore(() => new BoundedBuffer(-1));
        }

        [Test]
        public void ThrowsOnZeroCapacity()
        {
            Helpers.ThrowsAoore(() => new BoundedBuffer(0));
        }

        [Test]
        public void ThrowsOnNullInputBuffer()
        {
            var buffer = new BoundedBuffer();

            Helpers.ThrowsAne(() => buffer.AppendFill(null, 0, 0));
        }

        [Test]
        public void ThrowsOnNegativeSegmentOffset()
        {
            var buffer = new BoundedBuffer();
            var bytes = Helpers.Empty;

            Helpers.ThrowsAoore(() => buffer.AppendFill(bytes, -1, 0));
        }

        [Test]
        public void ThrowsOnNegativeSegmentLength()
        {
            var buffer = new BoundedBuffer();
            var bytes = Helpers.Empty;

            Helpers.ThrowsAoore(() => buffer.AppendFill(bytes, 0, -1));
        }

        [Test]
        public void ThrowsOnZeroSegmentLength()
        {
            var buffer = new BoundedBuffer();
            var bytes = Helpers.Empty;

            Helpers.ThrowsAoore(() => buffer.AppendFill(bytes, 0, 0));
        }

        [Test]
        public void ThrowsOnBadSegmentOffset()
        {
            var buffer = new BoundedBuffer();
            var bytes = Helpers.Empty;

            Helpers.ThrowsAse(() => buffer.AppendFill(bytes, 1, 1));
        }

        [Test]
        public void ThrowsOnBadSegmentLength()
        {
            var buffer = new BoundedBuffer();
            var bytes = Helpers.Empty;

            Helpers.ThrowsAse(() => buffer.AppendFill(bytes, 0, 1));
        }

        [Test]
        public void ThrowsOnNegativeNewCapacity()
        {
            var buffer = new BoundedBuffer(32);
            Helpers.ThrowsAoore(() => buffer.Reset(-1));
            Helpers.ThrowsAoore(() => buffer.ExtractAndReset(-1));
        }

        [Test]
        public void ThrowsOdeAfterDisposal()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();
            Helpers.ThrowsOde(() => buffer.AppendFill(Helpers.Empty, 0, 0));
            Helpers.ThrowsOde(() => buffer.Reset(0));
            Helpers.ThrowsOde(() => buffer.ExtractAndReset(0));
            Helpers.ThrowsOde(() => { var b = buffer.FreeSpace; });
        }

        [Test]
        public void DoesNotThrowOnDoubleDispose()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();
            buffer.Dispose();
        }

        [Test]
        public void DefaultHasNoCapacity()
        {
            var buffer = new BoundedBuffer();
            Assert.AreEqual(0, buffer.FreeSpace);
        }

        [Test]
        public void AllowsResettingToZeroCapacity()
        {
            var buffer = new BoundedBuffer(32);
            Assert.DoesNotThrow(() => buffer.Reset(0));
        }

        [Test]
        public void StartsWithRequestedCapacity()
        {
            const int RequestedCapacity = 123;
            var buffer = new BoundedBuffer(RequestedCapacity);

            Assert.AreEqual(RequestedCapacity, buffer.FreeSpace);
        }

        [Test]
        public void FreeSpaceDecreasesCorrectly()
        {
            const int Capacity = 20;
            const int ByteCount = 10;

            var buffer = new BoundedBuffer(Capacity);
            var bytes = Helpers.GetRandomBytes(ByteCount);

            int expected = buffer.FreeSpace - ByteCount;
            buffer.AppendFill(bytes, 0, ByteCount);
            int actual = buffer.FreeSpace;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FreeSpaceDecreasesUntilZero()
        {
            const int Capacity = 9;
            const int ByteCount = 10;

            var buffer = new BoundedBuffer(Capacity);
            var bytes = Helpers.GetRandomBytes(ByteCount);

            int expected = Math.Max(0, buffer.FreeSpace - ByteCount);
            buffer.AppendFill(bytes, 0, ByteCount);
            int actual = buffer.FreeSpace;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AppendsWholeArrayWhenThereIsSpace()
        {
            const int Capacity = 100;
            var buffer = new BoundedBuffer(Capacity);

            var appended = Helpers.GetRandomBytes(Capacity);
            buffer.AppendFill(appended, 0, Capacity);
            var extracted = buffer.ExtractAndReset(0);
            CollectionAssert.AreEqual(appended, extracted);
        }

        [Test]
        public void AppendsAsMuchAsPossibleWhenThereIsInsufficientSpace()
        {
            const int Capacity = 100;
            const int Extra = 27;
            var buffer = new BoundedBuffer(Capacity);

            var appended = Helpers.GetRandomBytes(Capacity + Extra);
            buffer.AppendFill(appended, 0, Capacity);

            var expected = appended.CopySegment(0, Capacity);
            var actual = buffer.ExtractAndReset(0);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
