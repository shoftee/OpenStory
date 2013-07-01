using System;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.IO;

namespace OpenStory.Tests.Common.IO
{
    [TestFixture]
    class PacketReaderFixtureBase
    {
        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Null_Buffer()
        {
            Action construction = () => new PacketReader((byte[])null);
            construction.ShouldThrow<ArgumentNullException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Null_Buffer_With_Zero_Length_Segment()
        {
            Action construction = () => new PacketReader(null, 0, 0);
            construction.ShouldThrow<ArgumentNullException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Construction_From_Null_Clone()
        {
            Action construction = () => new PacketReader((PacketReader)null);
            construction.ShouldThrow<ArgumentNullException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Negative_Offset()
        {
            Action construction = () => new PacketReader(new byte[10], -1, 0);
            construction.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Negative_Length()
        {
            Action construction = () => new PacketReader(new byte[10], 0, -1);
            construction.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Bad_Segment_Offset()
        {
            Action construction = () => new PacketReader(new byte[10], 11, 0);
            construction.ShouldThrow<ArraySegmentException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Throw_On_Bad_Segment_Length()
        {
            Action construction = () => new PacketReader(new byte[10], 0, 11);
            construction.ShouldThrow<ArraySegmentException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        [TestCase(10, 6, 5)]
        [TestCase(10, 5, 6)]
        [TestCase(10, 0, 11)]
        [TestCase(10, 11, 0)]
        [TestCase(10, 10, 1)]
        [TestCase(10, 1, 10)]
        public void Should_Throw_On_Bad_Segment_Length_Combination(int bufferLength, int segmentOffset, int segmentLength)
        {
            Action construction = () => new PacketReader(new byte[bufferLength], segmentOffset, segmentLength);
            construction.ShouldThrow<ArraySegmentException>();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Not_Throw_On_Non_Null_Buffer()
        {
            Action construction = () => new PacketReader(new byte[10]);
            construction.ShouldNotThrow();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Not_Throw_On_Non_Null_Buffer_With_Segment()
        {
            Action construction = () => new PacketReader(new byte[10], 2, 6);
            construction.ShouldNotThrow();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Not_Throw_On_Zero_Offset_And_Length_With_Empty_Buffer()
        {
            Action construction = () => new PacketReader(Helpers.Empty, 0, 0);
            construction.ShouldNotThrow();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Not_Throw_Zero_Offset_And_Length_With_Non_Empty_Buffer()
        {
            Action construction = () => new PacketReader(new byte[10], 0, 0);
            construction.ShouldNotThrow();
        }

        [Category("OpenStory.Common.IO.PacketReader.General")]
        [Test]
        public void Should_Not_Throw_On_Construction_From_Non_Null_Clone()
        {
            var reader = new PacketReader(new byte[10]);
            Action construction = () => new PacketReader(reader);
            construction.ShouldNotThrow();
        }
    }
}