using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Tests;

namespace OpenStory.Common.Tools
{
    [TestFixture]
    public sealed class ArraysFixture
    {
        #region Arrays.FastClone

        [Category("OpenStory.Common.IO.Tools.Arrays")]
        [Test]
        public void FastClone_Should_Throw_On_Null_Source()
        {
            Action nullCloning = () => ArrayExtensions.FastClone(null);
            nullCloning.ShouldThrow<ArgumentNullException>();
        }

        [Category("OpenStory.Common.IO.Tools.Arrays")]
        [Test]
        public void FastClone_Should_Return_Same_Values()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var clone = ArrayExtensions.FastClone(buffer);

            clone.Should().HaveSameCount(buffer);
            clone.Should().ContainInOrder(buffer);
        }

        [Category("OpenStory.Common.IO.Tools.Arrays")]
        [Test]
        public void FastClone_Changing_Cloned_Bytes_Should_Not_Change_Source()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var clone = ArrayExtensions.FastClone(buffer);

            clone[0]++;
            clone[0].Should().NotBe(buffer[0]);
        }

        #endregion

        #region Arrays.CopySegment

        [Category("OpenStory.Common.IO.Tools.Arrays")]
        [Test]
        public void CopySegment_Should_Throw_On_Null_Source()
        {
            Action nullCloning = () => ArrayExtensions.CopySegment(null, 0, 0);
            nullCloning.ShouldThrow<ArgumentNullException>();
        }

        [Category("OpenStory.Common.IO.Tools.Arrays")]
        [Test]
        public void CopySegment_Should_Clone_Only_Bytes_In_Segment()
        {
            var buffer = Helpers.GetRandomBytes(128);
            var segment = ArrayExtensions.CopySegment(buffer, 32, 64);

            segment.Should().HaveCount(64);
            segment.Should().ContainInOrder(buffer.Skip(32).Take(64));
        }

        #endregion
    }
}