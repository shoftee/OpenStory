using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using Moq;
using NUnit.Framework;

namespace OpenStory.Server.Tests
{
    [Category("OpenStory.Server")]
    [TestFixture]
    public class IvGeneratorFixture
    {
        [Test]
        public void GetNewIv_Should_Call_GetNonZeroBytes_Once()
        {
            // Arrange
            var rngMock = new Mock<RandomNumberGenerator>(MockBehavior.Loose);
            var generator = new IvGenerator(rngMock.Object);

            // Act
            generator.GetNewIv();

            // Assert
            Expression<Func<byte[], bool>> anEmptyIvBuffer
                = bytes => bytes.Length == 4 && bytes.All(b => b == 0);

            rngMock.Verify(rng => rng.GetNonZeroBytes(It.Is(anEmptyIvBuffer)), Times.Once());
        }
    }
}
