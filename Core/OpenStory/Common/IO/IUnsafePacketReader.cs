using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for unsafe packet reading.
    /// </summary>
    public interface IUnsafePacketReader
    {
        /// <summary>
        /// Gets the number of remaining bytes until the end of the buffer segment.
        /// </summary>
        int Remaining { get; }

        /// <summary>
        /// Advances the stream position by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        void Skip(int count);

        /// <summary>
        /// Reads a specified number of bytes.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <returns>an array containing the bytes read.</returns>
        byte[] ReadBytes(int count);

        /// <summary>
        /// Reads a <see cref="System.Byte"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        byte ReadByte();

        /// <summary>
        /// Reads a <see cref="System.Int16"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        short ReadInt16();

        /// <summary>
        /// Reads a <see cref="System.UInt16"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        ushort ReadUInt16();

        /// <summary>
        /// Reads a <see cref="System.Int32"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        int ReadInt32();

        /// <summary>
        /// Reads a <see cref="System.UInt32"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        uint ReadUInt32();

        /// <summary>
        /// Reads a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        long ReadInt64();

        /// <summary>
        /// Reads a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <returns>the value that was read from the stream.</returns>
        ulong ReadUInt64();

        /// <summary>
        /// Reads a length and a string from the stream.
        /// </summary>
        /// <returns>the string that was read from the stream.</returns>
        string ReadLengthString();

        /// <summary>
        /// Reads a null-terminated string and advances the position past the padding.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="paddingLength"/> is non-positive.
        /// </exception>
        /// <returns>the string that was read from the stream.</returns>
        string ReadPaddedString(int paddingLength);

        /// <summary>
        /// Reads a byte as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <remarks>
        /// The returned value is <c>true</c> if the read byte is not equal to <c>0</c>.
        /// </remarks>
        /// <returns>the value that was read from the stream.</returns>
        bool ReadBoolean();
    }
}