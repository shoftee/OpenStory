using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests
{
    [TestFixture]
    [Category("OpenStory.Common.IO.BoundedBuffer")]
    sealed class BoundedBufferFixture
    {
        [Test]
        public void Constructor_Should_Throw_On_Negative_Capacity()
        {
            Action action = () => new BoundedBuffer(-1);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Constructor_Should_Throw_On_Zero_Capacity()
        {
            Action action = () => new BoundedBuffer(0);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Null_Buffer()
        {
            var buffer = new BoundedBuffer();

            buffer.Invoking(b => b.AppendFill(null, 0, 0))
                  .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Negative_Offset()
        {
            var buffer = new BoundedBuffer();

            buffer.Invoking(b => b.AppendFill(Helpers.Empty, -1, 0))
                  .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Negative_Count()
        {
            var buffer = new BoundedBuffer();
            buffer.Invoking(b => b.AppendFill(Helpers.Empty, 0, -1))
                  .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Zero_Count()
        {
            var buffer = new BoundedBuffer();
            buffer.Invoking(b => b.AppendFill(Helpers.Empty, 0, 0))
                  .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Bad_Segment_Offset()
        {
            var buffer = new BoundedBuffer();
            buffer.Invoking(b => b.AppendFill(Helpers.Empty, 1, 1))
                  .ShouldThrow<ArraySegmentException>();
        }

        [Test]
        public void AppendFill_Should_Throw_On_Bad_Segment_Length()
        {
            var buffer = new BoundedBuffer();
            buffer.Invoking(b => b.AppendFill(Helpers.Empty, 0, 1))
                  .ShouldThrow<ArraySegmentException>();
        }

        [Test]
        public void Reset_Should_Throw_On_Negative_Capacity()
        {
            var buffer = new BoundedBuffer(32);
            
            buffer.Invoking(b => b.Reset(-1))
                  .ShouldThrow<ArgumentOutOfRangeException>();

            buffer.Invoking(b => b.ExtractAndReset(-1))
                  .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void AppendFill_Should_Throw_After_Disposal()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();
            buffer.Invoking(b => b.AppendFill(Helpers.Empty, 0, 0))
                  .ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        public void Reset_Should_Throw_After_Disposal()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();

            buffer.Invoking(b => b.Reset(0))
                  .ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        public void ExtractAndReset_Should_Throw_After_Disposal() 
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();

            buffer.Invoking(b => b.ExtractAndReset(0))
                  .ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        public void FreeSpace_Access_Should_Throw_After_Disposal()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();

            buffer.Invoking(b => { var freeSpace = b.FreeSpace; })
                  .ShouldThrow<ObjectDisposedException>();
        }

        [Test]
        public void Double_Dispose_Should_Not_Throw()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Dispose();

            buffer.Invoking(b => b.Dispose())
                  .ShouldNotThrow();
        }

        [Test]
        public void Default_Constructor_Should_Create_Instance_With_Default_Capacity()
        {
            var buffer = new BoundedBuffer();
            buffer.FreeSpace.Should().Be(0);
        }

        [Test]
        public void Reset_With_Zero_Capacity_Should_Not_Throw()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Invoking(b => b.Reset(0))
                  .ShouldNotThrow();
        }

        [Test]
        public void ExtractAndReset_With_Zero_Capacity_Should_Not_Throw()
        {
            var buffer = new BoundedBuffer(32);
            buffer.Invoking(b => b.ExtractAndReset(0))
                  .ShouldNotThrow();
        }

        [Test]
        public void Default_Constructor_Should_Create_Instance_With_Requested_Capacity()
        {
            var buffer = new BoundedBuffer(123);

            buffer.FreeSpace.Should().Be(123);
        }

        [Test]
        public void AppendFill_Should_Decrease_FreeSpace()
        {
            var buffer = new BoundedBuffer(30);
            var bytes = Helpers.GetRandomBytes(10);

            buffer.AppendFill(bytes, 0, 10);

            buffer.FreeSpace.Should().Be(20);
        }

        [Test]
        public void AppendFill_Should_Decrease_FreeSpace_Until_Zero()
        {
            var buffer = new BoundedBuffer(16);
            var bytes = Helpers.GetRandomBytes(32);

            buffer.AppendFill(bytes, 0, 32);

            buffer.FreeSpace.Should().Be(0);
        }

        [Test]
        public void AppendFill_Should_Return_Written_Bytes()
        {
            var buffer = new BoundedBuffer(30);
            var bytes = Helpers.GetRandomBytes(10);

            var written = buffer.AppendFill(bytes, 0, 10);

            written.Should().Be(10);
        }

        [Test]
        public void AppendFill_Should_Return_Written_Bytes_When_FreeSpace_Insufficient()
        {
            var buffer = new BoundedBuffer(9);
            var bytes = Helpers.GetRandomBytes(10);

            var written = buffer.AppendFill(bytes, 0, 10);

            written.Should().Be(9);
        }

        [Test]
        public void AppendFill_Should_Append_Whole_Array()
        {
            var buffer = new BoundedBuffer(128);

            var bytes = Helpers.GetRandomBytes(90);
            buffer.AppendFill(bytes, 0, 90);
            var extracted = buffer.ExtractAndReset(0);

            extracted.Take(90).Should().ContainInOrder(bytes);
        }

        [Test]
        public void AppendFill_Should_Fill_Buffer_When_FreeSpace_Insufficient()
        {
            var buffer = new BoundedBuffer(100);
            var bytes = Helpers.GetRandomBytes(127);
            buffer.AppendFill(bytes, 0, 100);

            var extracted = buffer.ExtractAndReset(0);

            extracted.Should().ContainInOrder(bytes.Take(100));
        }

        [Test]
        public void Reset_Should_Set_New_Capacity()
        {
            var buffer = new BoundedBuffer(100);

            buffer.Reset(64);

            buffer.FreeSpace.Should().Be(64);
        }

        [Test]
        public void ExtractAndReset_Should_Set_New_Capacity()
        {
            var buffer = new BoundedBuffer(100);

            buffer.ExtractAndReset(64);

            buffer.FreeSpace.Should().Be(64);
        }
    }
}
