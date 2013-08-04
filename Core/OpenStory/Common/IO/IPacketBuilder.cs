using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for building packets.
    /// </summary>
    public interface IPacketBuilder
    {
        /// <summary>
        /// Writes a <see cref="Byte"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteByte(byte number);

        /// <summary>
        /// Writes a <see cref="Int16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt16(short number);

        /// <summary>
        /// Writes a <see cref="UInt16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt16(ushort number);

        /// <summary>
        /// Writes a <see cref="Int32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt32(int number);

        /// <summary>
        /// Writes a <see cref="UInt32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt32(uint number);

        /// <summary>
        /// Writes a <see cref="Int64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt64(long number);

        /// <summary>
        /// Writes a <see cref="UInt64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt64(ulong number);

        /// <summary>
        /// Writes a specified number of 0-value bytes to the end of the packet.
        /// </summary>
        /// <param name="count">The number of bytes to write.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        void WriteZeroes(int count);

        /// <summary>
        /// Writes an array of bytes to the end of the packet.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="bytes"/> is <see langword="null"/>.
        /// </exception>
        void WriteBytes(byte[] bytes);

        /// <summary>
        /// Writes a <see cref="Boolean"/> to the end of the packet.
        /// </summary>
        /// <param name="boolean">The value to write.</param>
        void WriteBoolean(bool boolean);

        /// <summary>
        /// Writes a length-prefixed UTF-8 string to the end of the packet.
        /// </summary>
        /// <remarks>The length of the stream is written first.</remarks>
        /// <param name="string">The string to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="string"/> is <see langword="null"/>.
        /// </exception>
        void WriteLengthString(string @string);

        /// <summary>
        /// Writes a padded string to the end of the packet.
        /// </summary>
        /// <param name="string">The string to write.</param>
        /// <param name="paddingLength">The length to pad the string to.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="string"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="paddingLength"/> is a non-positive number.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="string"/> is not shorter than <paramref name="paddingLength"/>.
        /// </exception>
        void WritePaddedString(string @string, int paddingLength);
    }
}