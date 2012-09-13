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
    public sealed class PacketReader : ISafePacketReader, IUnsafePacketReader
    {
        private readonly byte[] buffer;
        private readonly int segmentStart;
        private readonly int segmentEnd;

        private int currentOffset;

        /// <inheritdoc />
        public int Remaining
        {
            get { return this.segmentEnd - this.currentOffset; }
        }

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
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

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
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            this.buffer = buffer;
            this.currentOffset = this.segmentStart = 0;
            this.segmentEnd = buffer.Length;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PacketReader"/>, with the same internal buffer segment and offset as an existing one.
        /// </summary>
        /// <param name="other">The <see cref="PacketReader"/> instance to clone.</param>
        public PacketReader(PacketReader other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            this.buffer = other.buffer;
            this.segmentStart = other.segmentStart;
            this.segmentEnd = other.segmentEnd;
            this.currentOffset = other.currentOffset;
        }

        #region Private methods

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

        /// <inheritdoc />
        public bool TrySkip(int count)
        {
            ThrowIfCountIsNegative(count);

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

        /// <inheritdoc />
        public int TrySkipTo(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "'offset' must be non-negative.");
            }

            int bufferOffset = this.segmentStart + offset;
            if (bufferOffset < this.currentOffset)
            {
                throw new ArgumentOutOfRangeException("offset", offset, "'offset' must be ahead of the current position.");
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

        /// <inheritdoc />
        public bool TryRead(int count, out byte[] array)
        {
            ThrowIfCountIsNegative(count);

            if (this.CanAdvance(count))
            {
                array = new byte[count];
                int start = this.UncheckedAdvance(count);
                Buffer.BlockCopy(this.buffer, start, array, 0, count);
                return true;
            }
            else
            {
                return MiscTools.Fail(out array);
            }
        }

        /// <inheritdoc />
        public bool TryReadByte(out byte value)
        {
            if (this.CanAdvance(1))
            {
                int index = this.UncheckedAdvance(1);
                value = this.buffer[index];
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadUInt32(out uint value)
        {
            if (this.CanAdvance(4))
            {
                int start = this.UncheckedAdvance(4);
                value = LittleEndianBitConverter.ToUInt32(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadInt32(out int value)
        {
            if (this.CanAdvance(4))
            {
                int start = this.UncheckedAdvance(4);
                value = LittleEndianBitConverter.ToInt32(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadUInt16(out ushort value)
        {
            if (this.CanAdvance(2))
            {
                int start = this.UncheckedAdvance(2);
                value = LittleEndianBitConverter.ToUInt16(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadInt16(out short value)
        {
            if (this.CanAdvance(2))
            {
                int start = this.UncheckedAdvance(2);
                value = LittleEndianBitConverter.ToInt16(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadInt64(out long value)
        {
            if (this.CanAdvance(8))
            {
                int start = this.UncheckedAdvance(8);
                value = LittleEndianBitConverter.ToInt64(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadUInt64(out ulong value)
        {
            if (this.CanAdvance(8))
            {
                int start = this.UncheckedAdvance(8);
                value = LittleEndianBitConverter.ToUInt64(this.buffer, start);
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadLengthString(out string result)
        {
            short length;
            if (this.TryReadInt16(out length) && this.CanAdvance(length))
            {
                int start = this.UncheckedAdvance(length);
                result = this.ReadString(start, length);
                return true;
            }
            else
            {
                return MiscTools.Fail(out result);
            }
        }

        /// <inheritdoc />
        public bool TryReadPaddedString(int length, out string result)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "'length' must be non-negative.");
            }

            if (this.CanAdvance(length))
            {
                int start = this.UncheckedAdvance(length);
                result = this.ReadNullTerminatedString(start, length);
                return true;
            }
            else
            {
                return MiscTools.Fail(out result);
            }
        }

        /// <inheritdoc />
        public bool TryReadBoolean(out bool value)
        {
            if (this.CanAdvance(1))
            {
                int index = this.UncheckedAdvance(1);
                value = this.buffer[index] != 0;
                return true;
            }
            else
            {
                return MiscTools.Fail(out value);
            }
        }

        #endregion

        #region Unsafe read methods

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public void Skip(int count)
        {
            ThrowIfCountIsNegative(count);

            this.CheckedAdvance(count);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public byte[] ReadBytes(int count)
        {
            ThrowIfCountIsNegative(count);

            int start = this.CheckedAdvance(count);
            var bytes = new byte[count];
            Buffer.BlockCopy(this.buffer, start, bytes, 0, count);
            return bytes;
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public byte ReadByte()
        {
            int index = this.CheckedAdvance(1);
            return this.buffer[index];
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public short ReadInt16()
        {
            int start = this.CheckedAdvance(2);
            return LittleEndianBitConverter.ToInt16(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public ushort ReadUInt16()
        {
            int start = this.CheckedAdvance(2);
            return LittleEndianBitConverter.ToUInt16(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public int ReadInt32()
        {
            int start = this.CheckedAdvance(4);
            return LittleEndianBitConverter.ToInt32(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public uint ReadUInt32()
        {
            int start = this.CheckedAdvance(4);
            return LittleEndianBitConverter.ToUInt32(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public long ReadInt64()
        {
            int start = this.CheckedAdvance(8);
            return LittleEndianBitConverter.ToInt64(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public ulong ReadUInt64()
        {
            int start = this.CheckedAdvance(8);
            return LittleEndianBitConverter.ToUInt64(this.buffer, start);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public string ReadLengthString()
        {
            short length = this.ReadInt16();
            int start = this.CheckedAdvance(length);
            return this.ReadString(start, length);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public string ReadPaddedString(int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length", length, "'length' must be a positive integer.");
            }

            int start = this.CheckedAdvance(length);
            return this.ReadNullTerminatedString(start, length);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public bool ReadBoolean()
        {
            int index = this.CheckedAdvance(1);
            return this.buffer[index] != 0;
        }

        #endregion

        /// <summary>
        /// Returns a byte array of the remaining data in the 
        /// stream and advances to the end of the stream.
        /// </summary>
        /// <returns>an array with the buffer's remaining data.</returns>
        public byte[] ReadFully()
        {
            int remaining = this.Remaining;
            var remainingBytes = new byte[remaining];

            int start = this.UncheckedAdvance(remaining);
            Buffer.BlockCopy(this.buffer, start, remainingBytes, 0, remaining);
            return remainingBytes;
        }

        #region Helpers

        /// <summary>
        /// Determines whether the position can safely advance by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to advance by.</param>
        /// <returns><c>true</c> if advancing is safe; otherwise, <c>false</c>.</returns>
        private bool CanAdvance(int count)
        {
            return this.currentOffset + count <= this.segmentEnd;
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
                throw PacketReadingException.EndOfStream();
            }

            int old = this.currentOffset;
            this.currentOffset += count;
            return old;
        }

        private static void ThrowIfCountIsNegative(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "'count' must be non-negative.");
            }
        }

        #endregion
    }
}
