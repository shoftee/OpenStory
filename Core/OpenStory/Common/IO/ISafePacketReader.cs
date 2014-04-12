using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for safe packet reading.
    /// </summary>
    public interface ISafePacketReader : IPacketReader
    {
        /// <summary>Attempts to advance the stream position by a specified number of bytes.</summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is negative.</exception>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TrySkip(int count);

        /// <summary>Attempts to advance to a new forward position.</summary>
        /// <param name="offset">The new position to assign, relative to the start of the segment.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is behind the current position of the stream.</exception>
        /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
        bool TrySkipTo(int offset);

        /// <summary>Attempts to read a specified number of bytes into a buffer.</summary>
        /// <remarks>On failure to read all bytes, <paramref name="array"/> is assigned <see langword="null"/>.</remarks>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="array">The buffer to store the bytes read in.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is negative.</exception>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryRead(int count, out byte[] array);

        /// <summary>Attempts to read a <see cref="Byte"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadByte(out byte value);

        /// <summary>Attempts to read a <see cref="UInt16"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadUInt16(out ushort value);

        /// <summary>Attempts to read a <see cref="Int16"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadInt16(out short value);

        /// <summary>Attempts to read a <see cref="Int32"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadInt32(out int value);

        /// <summary>Attempts to read a <see cref="UInt32"/> from the stream.</summary>
        /// <remarks> If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadUInt32(out uint value);

        /// <summary>Attempts to read a <see cref="Int64"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadInt64(out long value);

        /// <summary>Attempts to read a <see cref="UInt64"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadUInt64(out ulong value);

        /// <summary>Attempts to read a string with a provided length from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="result"/> is set to <see langword="null"/>.</remarks>
        /// <param name="result">A variable to hold the string.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadLengthString(out string result);

        /// <summary>Attempts to read a null-terminated padded string from the stream.</summary>
        /// <remarks>
        /// <para>If the read was unsuccessful, <paramref name="result"/> is set to <see langword="null"/>.</para>
        /// <para>Otherwise, <paramref name="result"/> contains the string read from the stream, 
        /// and the stream is advanced <paramref name="paddingLength"/> positions.</para>
        /// </remarks>
        /// <param name="paddingLength">The length of the padded string segment.</param>
        /// <param name="result">A variable to hold the string.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="paddingLength"/> is negative.</exception>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadPaddedString(int paddingLength, out string result);

        /// <summary>Attempts to read a byte as a <see cref="Boolean"/>.</summary>
        /// <remarks>
        /// <para>If the read was unsuccessful, <paramref name="value"/> is set to <see langword="false"/>.</para>
        /// <para>Otherwise, result is <see langword="true"/> if the byte that was read is not equal to <c>0</c>,
        /// and the stream is advanced by 1 position.</para>
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the read was successful; otherwise, <see langword="false"/>.</returns>
        bool TryReadBoolean(out bool value);
    }
}