using System.Security.Cryptography;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents an object that generates IV byte arrays.
    /// </summary>
    public class IvGenerator
    {
        private readonly RandomNumberGenerator rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="IvGenerator"/> class.
        /// </summary>
        /// <param name="rng">The random number generator to use.</param>
        public IvGenerator(RandomNumberGenerator rng)
        {
            this.rng = rng;
        }

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>a generated 4-byte IV array.</returns>
        public byte[] GetNewIv()
        {
            var iv = new byte[4];
            this.rng.GetNonZeroBytes(iv);
            return iv;
        }
    }
}
