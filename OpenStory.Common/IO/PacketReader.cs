using System;
using System.Text;
using OpenStory.Common.Tools;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a class for reading game packets.
    /// </summary>
    /// <remarks>
    /// This class exclusively uses little-endian byte order.
    /// </remarks>
    public class PacketReader
    {
        private readonly byte[] buffer;
        private readonly int segmentStart;
        private readonly int segmentEnd;

        private int currentOffset;

        /// <summary>
        /// Initializes a new instance of <see cref="PacketReader"/> using the given byte array segment as a buffer.
        /// </summary>
        /// <param name="buffer">The byte array to use as a buffer.</param>
        /// <param name="offset">The start of the buffer segment.</param>
        /// <param name="length">The length of the buffer segment.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if the array segment given by the <paramref name="offset"/> and 
        /// <paramref name="length"/> parameters falls outside of the array's bounds.
        /// </exception>
        public PacketReader(byte[] buffer, int offset, int length)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0 || buffer.Length < offset ||
                length <= 0 || offset + length > buffer.Length)
            {
                throw ArraySegmentException.GetByStartAndLength(offset, length);
            }

            this.buffer = buffer;
            this.currentOffset = this.segmentStart = offset;
            this.segmentEnd = offset + length;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PacketReader"/> over the given byte array.
        /// </summary>
        /// <param name="buffer">The byte array to read from.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer" /> is <c>null</c>.
        /// </exception>
        public PacketReader(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            this.buffer = buffer;
            this.currentOffset = this.segmentStart = 0;
            this.segmentEnd = buffer.Length;
        }

        #region Private methods

        /// <summary>
        /// Determines whether the position can safely advance by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to advance by.</param>
        /// <returns><c>true</c> if advancing is safe; otherwise, <c>false</c>.</returns>
        private bool CanAdvance(int count)
        {
            return !(this.currentOffset + count > this.segmentEnd);
        }

        /// <summary>Advances the stream by a number of positions and returns the original position.</summary>
        /// <param name="count">The number of positions to advance.</param>
        /// <returns>the position of the stream before advancing.</returns>
        private int UncheckedAdvance(int count)
        {
            int old = this.currentOffset;
            this.currentOffset += count;
            return old;
        }

        /// <summary>
        /// Advances the stream by a number of positions and returns the original position.
        /// </summary>
        /// <param name="count">The number of positions to advance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        /// <returns>the position of the stream before advancing.</returns>
        private int CheckedAdvance(int count)
        {
            if (this.currentOffset + count > this.segmentEnd)
            {
                throw new InvalidOperationException("You have reached the end of the stream.");
            }
            int old = this.currentOffset;
            this.currentOffset += count;
            return old;
        }

        /// <summary>Reads a null-terminated padded string from a position in the underlying array segment.</summary>
        /// <param name="start">The start of the padded string segment.</param>
        /// <param name="padLength">The total length of the string segment.</param>
        /// <returns>the string that was read.</returns>
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
        /// <returns>the string that was read.</returns>
        private string ReadString(int start, int length)
        {
            return Encoding.UTF8.GetString(this.buffer, start, length);
        }

        #endregion

        #region Safe read methods

        /// <summary>
        /// Attempts to advance the stream position by a specified number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TrySkip(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be non-negative.");
            }

            if (this.CanAdvance(count))
            {
                this.UncheckedAdvance(count);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Attempts to advance to a new forward position.
        /// </summary>
        /// <param name="offset">The new position to assign, relative to the start of the segment.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>Thrown if <paramref name="offset"/> is negative, </para>
        /// <para>OR, </para>
        /// <para><paramref name="offset"/> is behind the current position of the stream.</para>
        /// </exception>
        /// <returns>the number of bytes that were skipped if the operation was successful; otherwise, <c>-1</c>.</returns>
        public int TrySkipTo(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", "'offset' must be non-negative.");
            }

            int bufferOffset = this.segmentStart + offset;
            if (bufferOffset < this.currentOffset)
            {
                throw new ArgumentOutOfRangeException("offset", "'offset' must be ahead of the current position.");
            }

            int skipped = this.segmentEnd - bufferOffset;
            if (skipped < 0)
            {
                return -1;
            }
            else
            {
                this.currentOffset = bufferOffset;
                return skipped;
            }
        }

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
        public bool TryRead(int count, out byte[] array)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be non-negative.");
            }

            if (this.CanAdvance(count))
            {
                array = new byte[count];
                Buffer.BlockCopy(this.buffer, this.UncheckedAdvance(count), array, 0, count);
                return true;
            }
            else
            {
                return MiscTools.Fail(out array);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.Byte"/> from the stream.
        /// </summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadByte(out byte value)
        {
            if (this.CanAdvance(1))
            {
                value = this.buffer[this.UncheckedAdvance(1)];
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.UInt32"/> from the stream.
        /// </summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadUInt32(out uint value)
        {
            if (this.CanAdvance(4))
            {
                value = LittleEndianBitConverter.ToUInt32(this.buffer, this.UncheckedAdvance(4));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

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
        public bool TryReadInt32(out int value)
        {
            if (this.CanAdvance(4))
            {
                value = LittleEndianBitConverter.ToInt32(this.buffer, this.UncheckedAdvance(4));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.UInt16"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadUInt16(out ushort value)
        {
            if (this.CanAdvance(2))
            {
                value = LittleEndianBitConverter.ToUInt16(this.buffer, this.UncheckedAdvance(2));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.Int16"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadInt16(out short value)
        {
            if (this.CanAdvance(2))
            {
                value = LittleEndianBitConverter.ToInt16(this.buffer, this.UncheckedAdvance(2));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadInt64(out long value)
        {
            if (this.CanAdvance(8))
            {
                value = LittleEndianBitConverter.ToInt64(this.buffer, this.UncheckedAdvance(8));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a <see cref="System.UInt64"/> from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>0</c>.
        /// </remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadUInt64(out ulong value)
        {
            if (this.CanAdvance(8))
            {
                value = LittleEndianBitConverter.ToUInt64(this.buffer, this.UncheckedAdvance(8));
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <summary>
        /// Attempts to read a string with a provided length from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="result"/> is set to <c>null</c>.
        /// </remarks>
        /// <param name="result">A variable to hold the string.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadLengthString(out string result)
        {
            short length;
            if (this.TryReadInt16(out length) && this.CanAdvance(length))
            {
                result = this.ReadString(this.UncheckedAdvance(length), length);
                return true;
            }
            else
            {
                return MiscTools.Fail(out result);
            }
        }

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
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadPaddedString(int length, out string result)
        {
            if (this.CanAdvance(length))
            {
                result = this.ReadNullTerminatedString(this.UncheckedAdvance(length), length);
                return true;
            }
            else
            {
                return MiscTools.Fail(out result);
            }
        }

        /// <summary>
        /// Attempts to read a byte as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="value"/> is set to <c>false</c>.
        /// Otherwise, result is <c>true</c> if the byte that was read is not equal to 0,
        /// and the stream is advanced by 1 position.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the read was successful; otherwise, <c>false</c>.</returns>
        public bool TryReadBoolean(out bool value)
        {
            if (this.CanAdvance(1))
            {
                value = this.buffer[this.UncheckedAdvance(1)] != 0;
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        #endregion

        #region Quick read methods

        /// <summary>
        /// Advances the stream position by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        public void Skip(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be a non-negative integer.");
            }
            this.CheckedAdvance(count);
        }

        /// <summary>
        /// Reads a specified number of bytes.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>an array containing the bytes read.</returns>
        public byte[] ReadBytes(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be a non-negative integer.");
            }

            int start = this.CheckedAdvance(count);
            var bytes = new byte[count];
            Buffer.BlockCopy(this.buffer, start, bytes, 0, count);
            return bytes;
        }

        /// <summary>
        /// Reads a <see cref="System.Byte"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public byte ReadByte()
        {
            return this.buffer[this.CheckedAdvance(1)];
        }

        /// <summary>
        /// Reads a <see cref="System.Int16"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public short ReadInt16()
        {
            return LittleEndianBitConverter.ToInt16(this.buffer, this.CheckedAdvance(2));
        }

        /// <summary>
        /// Reads a <see cref="System.UInt16"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public ushort ReadUInt16()
        {
            return LittleEndianBitConverter.ToUInt16(this.buffer, this.CheckedAdvance(2));
        }

        /// <summary>
        /// Reads a <see cref="System.Int32"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public int ReadInt32()
        {
            return LittleEndianBitConverter.ToInt32(this.buffer, this.CheckedAdvance(4));
        }

        /// <summary>
        /// Reads a <see cref="System.UInt32"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public uint ReadUInt32()
        {
            return LittleEndianBitConverter.ToUInt32(this.buffer, this.CheckedAdvance(4));
        }

        /// <summary>
        /// Reads a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public long ReadInt64()
        {
            return LittleEndianBitConverter.ToInt64(this.buffer, this.CheckedAdvance(8));
        }

        /// <summary>
        /// Reads a <see cref="System.Int64"/> from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public ulong ReadUInt64()
        {
            return LittleEndianBitConverter.ToUInt64(this.buffer, this.CheckedAdvance(8));
        }

        /// <summary>
        /// Reads a length and a string from the stream.
        /// </summary>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the string that was read from the stream.</returns>
        public string ReadLengthString()
        {
            short length = this.ReadInt16();
            return this.ReadString(this.CheckedAdvance(length), length);
        }

        /// <summary>
        /// Reads a null-terminated string and advances the position past the padding.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="length"/> is non-positive.
        /// </exception>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the string that was read from the stream.</returns>
        public string ReadPaddedString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length", "'length' must be a positive integer.");
            }
            int stringStart = this.CheckedAdvance(length);
            return this.ReadNullTerminatedString(stringStart, length);
        }

        /// <summary>
        /// Reads a byte as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <remarks>
        /// The returned value is <c>true</c> if the read byte is not equal to <c>0</c>.
        /// </remarks>
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='InvalidOperationException']" />
        /// <returns>the value that was read from the stream.</returns>
        public bool ReadBoolean()
        {
            return this.buffer[this.CheckedAdvance(1)] != 0;
        }

        #endregion

        /// <summary>
        /// Gets the number of remaining bytes until the end of the buffer segment.
        /// </summary>
        public int Remaining
        {
            get { return this.segmentEnd - this.currentOffset; }
        }

        /// <summary>
        /// Returns a byte array of the remaining data in the 
        /// stream and advances to the end of the stream.
        /// </summary>
        /// <returns>an array with the buffer's remaining data.</returns>
        public byte[] ReadFully()
        {
            int remaining = this.Remaining;
            var remainingBytes = new byte[remaining];

            Buffer.BlockCopy(this.buffer, this.UncheckedAdvance(remaining), remainingBytes, 0, remaining);
            return remainingBytes;
        }
    }
}