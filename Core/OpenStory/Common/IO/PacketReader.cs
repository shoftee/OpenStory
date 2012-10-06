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
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="offset"/> or <paramref name="length"/> are negative.
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

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, Exceptions.OffsetMustBeNonNegative);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException("length", length, Exceptions.LengthMustBeNonNegative);
            }

            if (buffer.Length < offset || offset + length > buffer.Length)
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
        public bool TrySkipTo(int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", offset, Exceptions.OffsetMustBeNonNegative);
            }

            int bufferOffset = this.segmentStart + offset;
            if (bufferOffset < this.currentOffset)
            {
                throw new ArgumentOutOfRangeException("offset", offset, Exceptions.OffsetMustBeAheadOfCurrentPosition);
            }

            int remaining = this.segmentEnd - bufferOffset;
            if (remaining < 0)
            {
                return false;
            }
            else
            {
                this.currentOffset = bufferOffset;
                return true;
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
                return Misc.Fail(out array);
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
                return Misc.Fail(out value);
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
                return Misc.Fail(out value);
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
                return Misc.Fail(out value);
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
                return Misc.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadInt16(out short value)
        {
            return TryReadInt16(out value, false);
        }

        private bool TryReadInt16(out short value, bool peek)
        {
            if (this.CanAdvance(2))
            {
                int start = this.currentOffset;
                if (!peek)
                {
                    this.UncheckedAdvance(2);
                }

                value = LittleEndianBitConverter.ToInt16(this.buffer, start);
                return true;
            }
            else
            {
                return Misc.Fail(out value);
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
                return Misc.Fail(out value);
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
                return Misc.Fail(out value);
            }
        }

        /// <inheritdoc />
        public bool TryReadLengthString(out string result)
        {
            short length;
            if (!this.TryReadInt16(out length, peek: true))
            {
                return Misc.Fail(out result);
            }

            if (!this.CanAdvance(2 + length))
            {
                return Misc.Fail(out result);
            }

            this.UncheckedAdvance(2);

            int start = this.UncheckedAdvance(length);
            result = this.ReadString(start, length);
            return true;
        }

        /// <inheritdoc />
        public bool TryReadPaddedString(int paddingLength, out string result)
        {
            if (paddingLength <= 0)
            {
                throw new ArgumentOutOfRangeException("paddingLength", paddingLength, Exceptions.PaddingLengthMustBePositive);
            }

            if (this.CanAdvance(paddingLength))
            {
                int start = this.UncheckedAdvance(paddingLength);
                result = this.ReadNullTerminatedString(start, paddingLength);
                return true;
            }
            else
            {
                return Misc.Fail(out result);
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
                return Misc.Fail(out value);
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
            return this.ReadInt16(false);
        }

        private short ReadInt16(bool peek)
        {
            int start;
            if (peek)
            {
                // If we just wanna peek, don't advance.
                if (!this.CanAdvance(2))
                {
                    throw PacketReadingException.EndOfStream();
                }
                start = this.currentOffset;
            }
            else
            {
                // Otherwise do advance.
                start = this.CheckedAdvance(2);
            }

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
            // We do this in a convoluted fashion to avoid moving the position forward if we're gonna fail.
            short length = this.ReadInt16(true);

            if (!this.CanAdvance(2 + length))
            {
                throw PacketReadingException.EndOfStream();
            }

            string s = this.ReadString(this.currentOffset + 2, length);
            this.currentOffset += 2 + length;

            return s;
        }

        /// <inheritdoc />
        /// <inheritdoc cref="CheckedAdvance" select="exception[@cref='PacketReadingException']" />
        public string ReadPaddedString(int paddingLength)
        {
            if (paddingLength <= 0)
            {
                throw new ArgumentOutOfRangeException("paddingLength", paddingLength, Exceptions.PaddingLengthMustBePositive);
            }

            int start = this.CheckedAdvance(paddingLength);
            return this.ReadNullTerminatedString(start, paddingLength);
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
                throw new ArgumentOutOfRangeException("count", count, Exceptions.CountMustBeNonNegative);
            }
        }

        #endregion

        /// <summary>
        /// Wraps the current packet reader in a safety block.
        /// </summary>
        /// <remarks>
        /// On failure, this method will revert the position of the reader to what it was when the method was called.
        /// </remarks>
        /// <param name="readingCallback">The operation callback to execute on the reader.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="readingCallback"/> is <c>null</c>.</exception>
        /// <returns>whether the reading completed successfully.</returns>
        public bool Safe(Action<IUnsafePacketReader> readingCallback)
        {
            if (readingCallback == null)
            {
                throw new ArgumentNullException("readingCallback");
            }

            int start = this.currentOffset;
            bool success;
            try
            {
                readingCallback(this);
                success = true;
            }
            catch (PacketReadingException)
            {
                this.currentOffset = start;
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Wraps the current packet reader in a safety block, with a callback in case of failure.
        /// </summary>
        /// <remarks>
        /// On failure, this method will rever the position of the reader to what it was when the method was called.
        /// </remarks>
        /// <param name="readingCallback">The operation callback to execute on the reader.</param>
        /// <param name="failureCallback">The callback to execute on reading failure.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="readingCallback"/> or <paramref name="failureCallback"/> is <c>null</c>.</exception>
        /// <returns>whether the reading completed successfully.</returns>
        public bool Safe(Action<IUnsafePacketReader> readingCallback, Action failureCallback)
        {
            if (readingCallback == null)
            {
                throw new ArgumentNullException("readingCallback");
            }

            if (failureCallback == null)
            {
                throw new ArgumentNullException("failureCallback");
            }

            int start = this.currentOffset;
            bool success;
            try
            {
                readingCallback(this);
                success = true;
            }
            catch (PacketReadingException)
            {
                this.currentOffset = start;
                success = false;
            }

            if (!success)
            {
                failureCallback();
            }

            return success;
        }

        /// <summary>
        /// Wraps the current packet reader in a safety block, with a callback in case of failure.
        /// </summary>
        /// <remarks>
        /// On failure, this method will rever the position of the reader to what it was when the method was called.
        /// </remarks>
        /// <param name="readingCallback">The operation callback to execute on the reader.</param>
        /// <param name="failureCallback">The callback to execute on reading failure.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="readingCallback"/> or <paramref name="failureCallback"/> is <c>null</c>.</exception>
        /// <returns>on success, the value returned by the reading callback; otherwise, the value returned by the failure callback.</returns>
        public T Safe<T>(Func<IUnsafePacketReader, T> readingCallback, Func<T> failureCallback)
        {
            if (readingCallback == null)
            {
                throw new ArgumentNullException("readingCallback");
            }

            if (failureCallback == null)
            {
                throw new ArgumentNullException("failureCallback");
            }

            int start = this.currentOffset;
            var result = default(T);
            bool success;
            try
            {
                result = readingCallback(this);
                success = true;
            }
            catch (PacketReadingException)
            {
                this.currentOffset = start;
                success = false;
            }

            if (!success)
            {
                result = failureCallback();
            }

            return result;
        }
    }
}
