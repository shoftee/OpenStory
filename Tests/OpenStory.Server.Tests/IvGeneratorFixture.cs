using System.Linq;
using System.Security.Cryptography;
using Moq;
using NUnit.Framework;

namespace OpenStory.Server
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
            rngMock.Verify(rng => rng.GetNonZeroBytes(ZeroByteArrayWithLength4()), Times.Once());
        }

        private static byte[] ZeroByteArrayWithLength4()
        {
            return It.Is<byte[]>(bytes => bytes.Length == 4 && bytes.All(b => b == 0));
        }
    }
}
