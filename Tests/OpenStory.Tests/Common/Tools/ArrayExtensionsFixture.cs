using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OpenStory.Common.IO;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common
{
    [Category("OpenStory.Common.ArrayExtensions")]
    [TestFixture]
    public sealed class ArrayExtensionsFixture
    {
        #region FastClone()

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

        #region CopySegment

        [Test]
        public void CopySegment_Should_Throw_On_Null_Source()
        {
            Action nullCloning = () => ((byte[])null).CopySegment(0, 0);
            nullCloning.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void CopySegment_Should_Throw_On_Invalid_Offset()
        {
            var array = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            array
                .Invoking(a => a.CopySegment(-1, 1))
                .ShouldThrow<ArgumentException>();
        }

        [Test]
        public void CopySegment_Should_Throw_On_Invalid_Length()
        {
            var array = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            array
                .Invoking(a => a.CopySegment(1, -1))
                .ShouldThrow<ArgumentException>();
        }

        [TestCase(0, 5)]
        [TestCase(1, 5)]
        [TestCase(4, 1)]
        public void CopySegment_Should_Throw_On_Invalid_Segment(int offset, int length)
        {
            var array = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            array
                .Invoking(a => a.CopySegment(offset, length))
                .ShouldThrow<ArraySegmentException>();
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

        #region Reverse<T>()

        [Test]
        public void Reverse_Should_Throw_On_Null_Array()
        {
            Action nullReverse = () => ArrayExtensions.Reverse((byte[])null);

            nullReverse
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Reverse_Should_Return_Reversed_Array()
        {
            var array = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            var reversedArray = ArrayExtensions.Reverse(array);

            reversedArray.Should().HaveCount(array.Length);
            reversedArray.Should().ContainInOrder(new byte[] { 0x78, 0x56, 0x34, 0x12 });
        }

        #endregion

        #region Reverse<T>()

        [Test]
        public void ReverseInPlace_Should_Throw_On_Null_Array()
        {
            Action nullReverseInPlace = () => ArrayExtensions.ReverseInPlace((byte[])null);

            nullReverseInPlace
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ReverseInPlace_Should_Reverse_Specified_Array_In_Place()
        {
            var array = new byte[] { 0x12, 0x34, 0x56, 0x78 };

            ArrayExtensions.ReverseInPlace(array);
            
            array.Should().ContainInOrder(new byte[] { 0x78, 0x56, 0x34, 0x12 });
        }

        #endregion
    }
}
