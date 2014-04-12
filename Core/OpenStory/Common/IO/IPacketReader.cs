namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides simple packet reader methods.
    /// </summary>
    public interface IPacketReader
    {
        /// <summary>
        /// Gets the number of remaining bytes until the end of the buffer segment.
        /// </summary>
        int Remaining { get; }

        /// <summary>
        /// Returns a byte array of the remaining data in the 
        /// stream and advances to the end of the stream.
        /// </summary>
        /// <returns>an array with the buffer's remaining data.</returns>
        byte[] ReadFully();
    }
}