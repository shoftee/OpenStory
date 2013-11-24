using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.Tools.Arrays")]
    [TestFixture]
    public sealed class ArraysFixture
    {
        #region Arrays.FastClone

        [Test]
        public void FastClone_Should_Throw_On_Null_Source()
        {
            Action nullCloning = () => ((byte[])null).FastClone();
            nullCloning.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void FastClone_Should_Return_Same_Values()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var clone = buffer.FastClone();

            clone.Should().HaveSameCount(buffer);
            clone.Should().ContainInOrder(buffer);
        }

        [Test]
        public void FastClone_Changing_Cloned_Bytes_Should_Not_Change_Source()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var clone = buffer.FastClone();

            clone[0]++;
            clone[0].Should().NotBe(buffer[0]);
        }

        #endregion

        #region Arrays.CopySegment

        [Test]
        public void CopySegment_Should_Throw_On_Null_Source()
        {
            Action nullCloning = () => ((byte[])null).CopySegment(0, 0);
            nullCloning.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void CopySegment_Should_Clone_Only_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var segment = buffer.CopySegment(32, 64);

            segment.Should().HaveCount(64);
            segment.Should().ContainInOrder(buffer.Skip(32).Take(64));
        }

        #endregion
    }
}