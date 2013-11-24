using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OpenStory.Tests;

namespace OpenStory.Cryptography
{
    [Category("OpenStory.Common.Cryptography")]
    [TestFixture]
    public sealed class RollingIvFixture
    {
        [Test]
        public void Constructor_Throws_On_Null_Algorithm()
        {
            Action action = () => new RollingIv(null, new byte[0], 0x0000);
            action
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructor_Throws_On_Null_InitialIv()
        {
            var algorithm = Mock.Of<ICryptoAlgorithm>();
            Action action = () => new RollingIv(algorithm, null, 0x0000);
            action
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructor_Throws_When_InitialIv_Length_Is_Not_4()
        {
            var algorithm = Mock.Of<ICryptoAlgorithm>();

            Action ivLength3 = () => new RollingIv(algorithm, new byte[3], 0x0000);
            ivLength3
                .ShouldThrow<ArgumentException>()
                .WithMessageSubstring(CommonStrings.IvMustBe4Bytes);

            Action ivLength5 = () => new RollingIv(algorithm, new byte[5], 0x0000);
            ivLength5
                .ShouldThrow<ArgumentException>()
                .WithMessageSubstring(CommonStrings.IvMustBe4Bytes);
        }

        [Test]
        public void Transform_Throws_On_Null_Buffer()
        {
            var algorithm = Mock.Of<ICryptoAlgorithm>();
            var rollingIv = new RollingIv(algorithm, new byte[] { 0x12, 0x34, 0x56, 0x78 }, 0x1234);
            rollingIv
                .Invoking(iv => iv.Transform(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Transform_Calls_TransformArraySegment_And_ShuffleIv()
        {
            var algorithm = new Mock<ICryptoAlgorithm>(MockBehavior.Loose);
            var initialIv = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            var rollingIv = new RollingIv(algorithm.Object, initialIv, 0x1234);
            var data = new byte[] { 0x12, 0x34 };

            rollingIv.Transform(data);

            algorithm.Verify(m => m.TransformArraySegment(AnyByteArray(), AnyByteArray(), 0, 2), Times.Once());
            algorithm.Verify(m => m.ShuffleIv(AnyByteArray()), Times.Once());
        }

        private static byte[] AnyByteArray()
        {
            return It.IsAny<byte[]>();
        }

        [Test]
        public void ConstructHeader_Throws_For_Packets_Shorter_Than_2_Bytes()
        {
            var algorithm = new Mock<ICryptoAlgorithm>(MockBehavior.Loose);
            var initialIv = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            var rollingIv = new RollingIv(algorithm.Object, initialIv, 0x1234);

            rollingIv
                .Invoking(iv => iv.ConstructHeader(1).Whatever())
                .ShouldThrow<ArgumentOutOfRangeException>()
                .WithMessageSubstring(CommonStrings.PacketLengthMustBeMoreThan2Bytes);
        }

        [Test]
        public void GetPacketHeader_Throws_For_Null_Segment()
        {
            Action getPacketLength = () => RollingIv.GetPacketLength(null);
            getPacketLength
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ValidateHeader_Throws_On_Null_Segment()
        {
            var algorithm = new Mock<ICryptoAlgorithm>(MockBehavior.Loose);
            var initialIv = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            var rollingIv = new RollingIv(algorithm.Object, initialIv, 0x1234);

            rollingIv
                .Invoking(iv => iv.ValidateHeader(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void GetPacketHeader_Throws_When_Segment_Shorter_Than_4_Bytes()
        {
            Action getPacketLength = () => RollingIv.GetPacketLength(new byte[] { 0x01, 0x02, 0x03 });
            getPacketLength
                .ShouldThrow<ArgumentException>()
                .WithMessageSubstring(SegmentMustBeLongerThan4());
        }

        [Test]
        public void ValidateHeader_Throws_When_Segment_Shorter_Than_4_Bytes()
        {
            var algorithm = new Mock<ICryptoAlgorithm>(MockBehavior.Loose);
            var initialIv = new byte[] { 0x12, 0x34, 0x56, 0x78 };
            var rollingIv = new RollingIv(algorithm.Object, initialIv, 0x1234);

            rollingIv
                .Invoking(iv => iv.ValidateHeader(new byte[] { 0x01, 0x02, 0x03 }))
                .ShouldThrow<ArgumentException>()
                .WithMessageSubstring(SegmentMustBeLongerThan4());
        }

        private static string SegmentMustBeLongerThan4()
        {
            return string.Format(CommonStrings.SegmentTooShort, 4);
        }
    }
}
