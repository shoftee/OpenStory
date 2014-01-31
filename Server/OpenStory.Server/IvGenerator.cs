using System.Security.Cryptography;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents an object that generates IV byte arrays.
    /// </summary>
    public sealed class IvGenerator
    {
        private readonly RandomNumberGenerator randomNumberGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="IvGenerator"/> class.
        /// </summary>
        /// <param name="randomNumberGenerator">The random number generator to use.</param>
        public IvGenerator(RandomNumberGenerator randomNumberGenerator)
        {
            this.randomNumberGenerator = randomNumberGenerator;
        }

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>a generated 4-byte IV array.</returns>
        public byte[] GetNewIv()
        {
            var iv = new byte[4];
            this.randomNumberGenerator.GetNonZeroBytes(iv);
            return iv;
        }
    }
}
