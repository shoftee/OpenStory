using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for safe packet reading.
    /// </summary>
    public interface ISafePacketReader
    {
        /// <summary>
        /// Gets the number of remaining bytes until the end of the buffer segment.
        /// </summary>
        int Remaining { get; }

        /// <summary>
        /// Attempts to advance the stream position by a specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TrySkip(int count);

        /// <summary>
        /// Attempts to advance to a new forward position.
        /// </summary>
        /// <param name="offset">The new position to assign, relative to the start of the segment.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is behind the current position of the stream.</exception>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        bool TrySkipTo(int offset);

        /// <summary>
        /// Attempts to read a specified number of bytes into a buffer.
        /// </summary>
        /// <remarks>
        /// On failure to read all bytes, <paramref name="array"/> is assigned <c>null</c>.
        /// </remarks>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="array">The buffer to store the bytes read in.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryRead(int count, out byte[] array);

        /// <summary>
        /// Attempts to read a <see cref="System.Byte"/> from the stream.
        /// </summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadByte(out byte value);

        /// <summary>
        /// Attempts to read a <see cref="System.UInt32"/> from the stream.
        /// </summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadUInt32(out uint value);

        /// <summary>
        /// Attempts to read a <see cref="System.Int32"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>
        /// <c>true</c> if the read was successful; otherwise, <c>false</c>.
        /// </returns>
        bool TryReadInt32(out int value);

        /// <summary>
        /// Attempts to read a <see cref="System.UInt16"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadUInt16(out ushort value);

        /// <summary>
        /// Attempts to read a <see cref="System.Int16"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadInt16(out short value);

        /// <summary>
        /// Attempts to read a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadInt64(out long value);

        /// <summary>
        /// Attempts to read a <see cref="System.UInt64"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadUInt64(out ulong value);

        /// <summary>
        /// Attempts to read a string with a provided length from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="result"/> is set to <c>null</c>.
        /// </remarks>
        /// <param name="result">A variable to hold the string.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadLengthString(out string result);

        /// <summary>
        /// Attempts to read a null-terminated padded string from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="result"/> is set to <c>null</c>.
        /// Otherwise, <paramref name="result"/> contains the string read from the stream, 
        /// and the stream is advanced <paramref name="length"/> positions.
        /// </remarks>
        /// <param name="length">The length of the padded string segment.</param>
        /// <param name="result">A variable to hold the string.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="length"/> is negative.
        /// </exception>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadPaddedString(int length, out string result);

        /// <summary>
        /// Attempts to read a byte as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>false</c>.
        /// Otherwise, result is <c>true</c> if the byte that was read is not equal to 0,
        /// and the stream is advanced by 1 position.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        bool TryReadBoolean(out bool value);
    }
}