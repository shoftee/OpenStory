using System;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>Represents a big-endian stream reader.</summary>
    public class PacketReader
    {
        private byte[] buffer;

        private int end;
        private int position;

        /// <summary>Initializes a new instance of PacketReader on the given byte array segment.</summary>
        /// <param name="buffer">The byte array to use.</param>
        /// <param name="startPosition">The start of the read segment.</param>
        /// <param name="length">The length of the read segment.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="buffer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The exception is thrown if <paramref name="length"/> is non-positive, 
        /// OR, if <paramref name="startPosition"/> is non-positive or outside the bounds of the array,
        /// OR, if the end of the segment falls outside the bounds of the array.
        /// </exception>
        public PacketReader(byte[] buffer, int startPosition, int length)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (startPosition < 0 || buffer.Length < startPosition)
            {
                throw new ArgumentOutOfRangeException("startPosition");
            }
            if (length <= 0 || startPosition + length > buffer.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            this.buffer = buffer;
            this.position = startPosition;
            this.end = startPosition + length;
        }

        /// <summary>Initializes a new instance of PacketReader over the given byte array.</summary>
        /// <param name="buffer">The byte array to read from.</param>
        public PacketReader(byte[] buffer)
            : this(buffer, 0, buffer.Length) {}

        #region Private methods

        /// <summary>
        /// Determines whether the position can safely advance by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to advance by.</param>
        /// <returns>true if advance is possible; otherwise, false.</returns>
        private bool CanAdvance(int count)
        {
            return !(this.position + count > this.end);
        }

        /// <summary>Advances the stream by a number of positions and returns the original position.</summary>
        /// <param name="count">The number of positions to advance.</param>
        /// <returns>The position of the stream before advancing.</returns>
        private int UncheckedAdvance(int count)
        {
            int old = this.position;
            this.position += count;
            return old;
        }

        /// <summary>Advances the stream by a number of positions and returns the original position.</summary>
        /// <param name="count">The number of positions to advance.</param>
        /// <returns>The position of the stream before advancing.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        private int CheckedAdvance(int count)
        {
            if (this.position + count > this.end)
            {
                throw new InvalidOperationException("You have reached the end of the stream.");
            }
            int old = this.position;
            this.position += count;
            return old;
        }

        /// <summary>Reads a null-terminated padded string from a position in the underlying array segment.</summary>
        /// <param name="start">The start of the padded string segment.</param>
        /// <param name="padLength">The total length of the string segment.</param>
        /// <returns>The string that was read.</returns>
        private string ReadNullTerminatedString(int start, int padLength)
        {
            int count = 0;
            int currentPosition = start;
            while (this.buffer[currentPosition] != 0 && count < padLength - 1)
            {
                count++;
                currentPosition++;
            }
            return this.ReadString(start, count);
        }

        /// <summary>
        /// Reads a string with a set length from a position in the underlying array segment.
        /// </summary>
        /// <param name="start">The start of the string segment.</param>
        /// <param name="length">The length of the string segment.</param>
        /// <returns>The string that was read.</returns>
        private string ReadString(int start, int length)
        {
            return Encoding.UTF8.GetString(this.buffer, start, length);
        }

        #endregion

        #region Safe read methods

        /// <summary>Attempts to advance the stream position by a number of bytes.</summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <returns>true if the operation was successful; otherwise, false.</returns>
        public bool TrySkip(int count)
        {
            if (!this.CanAdvance(count)) return false;
            this.position += count;
            return true;
        }

        /// <summary>Attempts to read a <see cref="System.Byte"/> from the stream.</summary>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        public bool TryReadByte(out byte value)
        {
            value = 0;
            if (!this.CanAdvance(1)) return false;
            value = this.buffer[this.UncheckedAdvance(1)];
            return true;
        }

        /// <summary>Attempts to read a <see cref="System.Int16"/> from the stream.</summary>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        public bool TryReadInt16(out short value)
        {
            value = 0;
            if (!this.CanAdvance(2)) return false;
            value = BigEndianBitConverter.ToInt16(this.buffer, this.UncheckedAdvance(2));
            return true;
        }

        /// <summary>Attempts to read a <see cref="System.Int32"/> from the stream.</summary>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        public bool TryReadInt32(out int value)
        {
            value = 0;
            if (!this.CanAdvance(4)) return false;
            value = BigEndianBitConverter.ToInt16(this.buffer, this.UncheckedAdvance(4));
            return true;
        }

        /// <summary>Attempts to read a <see cref="System.Int64"/> from the stream.</summary>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        public bool TryReadInt64(out long value)
        {
            value = 0;
            if (!this.CanAdvance(8)) return false;
            value = BigEndianBitConverter.ToInt64(this.buffer, this.UncheckedAdvance(8));
            return true;
        }

        /// <summary>Attempts to read a string with a provided length from the stream.</summary>
        /// <param name="str">A variable to hold the string.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>If the read was unsuccessful, <paramref name="str"/> is set to <see cref="String.Empty"/>.</remarks>
        public bool TryReadLengthString(out string str)
        {
            str = String.Empty;
            short length;
            if (!this.TryReadInt16(out length) || !this.CanAdvance(length)) return false;
            str = this.ReadString(this.UncheckedAdvance(length), length);
            return true;
        }

        /// <summary>Attempts to read a null-terminated padded string from the stream.</summary>
        /// <param name="length">The length of the padded string segment.</param>
        /// <param name="str">A variable to hold the string.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="str"/> is set to <see cref="String.Empty"/>.
        /// Otherwise, <paramref name="str"/> contains the string read from the stream, 
        /// and the stream is advanced <paramref name="length"/> positions.
        /// </remarks>
        public bool TryReadPaddedString(int length, out string str)
        {
            str = String.Empty;
            if (!this.CanAdvance(length)) return false;
            str = this.ReadNullTerminatedString(this.UncheckedAdvance(length), length);
            return true;
        }

        #endregion

        #region Quick read methods

        /// <summary>
        /// Advances the stream position by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public void Skip(int count)
        {
            this.CheckedAdvance(count);
        }

        /// <summary>Reads a <see cref="System.Byte"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public byte ReadByte()
        {
            return this.buffer[this.CheckedAdvance(1)];
        }

        /// <summary>Reads a <see cref="System.Int16"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public short ReadInt16()
        {
            return BigEndianBitConverter.ToInt16(this.buffer, this.CheckedAdvance(2));
        }

        /// <summary>Reads a <see cref="System.Int32"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public int ReadInt32()
        {
            return BigEndianBitConverter.ToInt32(this.buffer, this.CheckedAdvance(4));
        }

        /// <summary>Reads a <see cref="System.Int64"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public long ReadInt64()
        {
            return BigEndianBitConverter.ToInt64(this.buffer, this.CheckedAdvance(8));
        }

        /// <summary>Reads a length and a string from the stream.</summary>
        /// <returns>The string that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public string ReadLengthString()
        {
            short length = this.ReadInt16();
            return this.ReadString(this.CheckedAdvance(length), length);
        }

        /// <summary>Reads a null-terminated string and advances the position past the padding.</summary>
        /// <returns>The string that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public string ReadPaddedString(int length)
        {
            int start = this.CheckedAdvance(length);
            return this.ReadNullTerminatedString(start, length);
        }

        #endregion
    }
}