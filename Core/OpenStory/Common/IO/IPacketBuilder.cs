using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for building packets.
    /// </summary>
    public interface IPacketBuilder
    {
        /// <summary>
        /// Writes a <see cref="System.Int64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt64(long number);

        /// <summary>
        /// Writes a <see cref="System.UInt64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt64(ulong number);

        /// <summary>
        /// Writes a <see cref="System.Int32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt32(int number);

        /// <summary>
        /// Writes a <see cref="System.UInt32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt32(uint number);

        /// <summary>
        /// Writes a <see cref="System.Int16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt16(short number);

        /// <summary>
        /// Writes a <see cref="System.UInt16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteInt16(ushort number);

        /// <summary>
        /// Writes a <see cref="System.Byte"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        void WriteByte(byte number);

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
        /// Thrown if <paramref name="bytes"/> is <c>null</c>.
        /// </exception>
        void WriteBytes(byte[] bytes);

        /// <summary>
        /// Writes a <see cref="System.Boolean"/> to the end of the packet.
        /// </summary>
        /// <param name="boolean">The value to write.</param>
        void WriteBoolean(bool boolean);

        /// <summary>
        /// Writes a length-prefixed UTF8 string to the end of the packet.
        /// </summary>
        /// <remarks>The length of the stream is written first.</remarks>
        /// <param name="s">The string to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="s"/> is <c>null</c>.
        /// </exception>
        void WriteLengthString(string s);

        /// <summary>
        /// Writes a padded string to the end of the packet.
        /// </summary>
        /// <param name="s">The string to write.</param>
        /// <param name="padLength">The length to pad the string to.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="padLength"/> is a non-positive number.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="s"/> is longer than <paramref name="padLength"/>.
        /// </exception>
        void WritePaddedString(string s, int padLength);
    }
}