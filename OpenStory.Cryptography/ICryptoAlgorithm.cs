using System;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides methods for cryptographic transformation.
    /// </summary>
    public interface ICryptoAlgorithm
    {
        /// <summary>
        /// Performs the shuffle operation on a specified IV.
        /// </summary>
        /// <param name="iv">The IV to shuffle.</param>
        /// <returns>the shuffled IV.</returns>
        byte[] ShuffleIv(byte[] iv);

        /// <summary>
        /// Transforms a byte array segment in-place.
        /// </summary>
        /// <remarks>
        /// The array specified used for the <paramref name="iv"/> argument will not be modified.
        /// </remarks>
        /// <param name="data">The byte containing the segment.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="segmentStart">The offset of the start of the segment.</param>
        /// <param name="segmentEnd">The offset of the end of the segment.</param>
        void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd);

        /// <summary>
        /// Transforms an array with a specified IV.
        /// </summary>
        /// <remarks>
        /// The specified arrays are not modified.
        /// </remarks>
        /// <param name="data">The data to transform.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the parameters are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="iv"/> does not have exactly 4 elements.
        /// </exception>
        /// <returns>a transformed copy of the array.</returns>
        byte[] TransformWithIv(byte[] data, byte[] iv);
    }
}
