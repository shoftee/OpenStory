using System;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>Represents a big-endian stream reader.</summary>
    public class PacketReader
    {
        private byte[] buffer;

        private int segmentStart;
        private int segmentEnd;
        private int currentOffset;

        /// <summary>
        /// The number of remaining bytes until the end of the buffer segment.
        /// </summary>
        public int Remaining
        {
            get { return this.segmentEnd - this.currentOffset; }
        }

        /// <summary>
        /// Initializes a new instance of PacketReader 
        /// using the given byte array segment as a buffer.
        /// </summary>
        /// <param name="buffer">The byte array to use as a buffer.</param>
        /// <param name="offset">The start of the buffer segment.</param>
        /// <param name="length">The length of the buffer segment.</param>
        /// <exception cref="ArgumentNullException">
        /// The exception is thrown if <paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The exception is thrown 
        /// if <paramref name="length"/> is non-positive, 
        /// OR, 
        /// if <paramref name="offset"/> is outside the array bounds.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The exception is thrown if the end of the segment doesn't fit the array bounds.
        /// </exception>
        public PacketReader(byte[] buffer, int offset, int length)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            if (offset < 0 || buffer.Length < offset)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length");
            } 
            if (offset + length > buffer.Length)
            {
                throw new ArgumentException("length");
            }

            this.buffer = buffer;
            this.currentOffset = this.segmentStart = offset;
            this.segmentEnd = offset + length;
        }

        /// <summary>Initializes a new instance of PacketReader over the given byte array.</summary>
        /// <param name="buffer">The byte array to read from.</param>
        public PacketReader(byte[] buffer) : this(buffer, 0, buffer.Length) { }

        #region Private methods

        /// <summary>
        /// Determines whether the position can safely advance by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to advance by.</param>
        /// <returns>true if advance is possible; otherwise, false.</returns>
        private bool CanAdvance(int count)
        {
            return !(this.currentOffset + count > this.segmentEnd);
        }

        /// <summary>Advances the stream by a number of positions and returns the original position.</summary>
        /// <param name="count">The number of positions to advance.</param>
        /// <returns>The position of the stream before advancing.</returns>
        private int UncheckedAdvance(int count)
        {
            int old = this.currentOffset;
            this.currentOffset += count;
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

        /// <summary>
        /// Attempts to advance the stream position by a number of bytes.
        /// </summary>
        /// <param name="count">The number of bytes to skip.</param>
        /// <returns>true if the operation was successful; otherwise, false.</returns>
        public bool TrySkip(int count)
        {
            if (!this.CanAdvance(count)) return false;
            this.currentOffset += count;
            return true;
        }

        /// <summary>
        /// Attempts to advance to a new forward position.
        /// </summary>
        /// <param name="offset">The new position to assign.</param>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if <paramref name="offset"/> 
        /// is behind the current position.
        /// </exception>
        /// <returns>
        /// If the operation was successful, the number 
        /// of bytes that were skipped. Otherwise, -1.
        /// </returns>
        public int TrySkipTo(int offset)
        {
            int bufferOffset = this.segmentStart + offset;
            if (bufferOffset < this.currentOffset)
            {
                throw new InvalidOperationException("You can't skip backwards.");
            }

            int skipped = this.segmentEnd - bufferOffset;
            if (skipped < 0) return -1;
            this.currentOffset = bufferOffset;
            return skipped;
        }

        /// <summary>Attempts to read a number of bytes into a buffer.</summary>
        /// <remarks>
        /// On failure to read all bytes, 
        /// <paramref name="array"/> is assigned null.
        /// </remarks>
        /// <param name="count">The number of bytes to read.</param>
        /// <param name="array">The buffer to store the bytes read in.</param>
        /// <returns>true if all bytes were read; otherwise, false.</returns>
        public bool TryRead(int count, out byte[] array)
        {
            if (!this.CanAdvance(count)) goto Fail;
            array = new byte[count];
            Buffer.BlockCopy(this.buffer, this.UncheckedAdvance(count), array, 0, count);

        Fail:
            array = null;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.Byte"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadByte(out byte value)
        {
            if (!this.CanAdvance(1)) goto Fail;
            value = this.buffer[this.UncheckedAdvance(1)];
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.UInt32"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadUInt32(out uint value)
        {
            if (!this.CanAdvance(4)) goto Fail;
            value = BigEndianBitConverter.ToUInt32(this.buffer, this.UncheckedAdvance(4));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.Int32"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadInt32(out int value)
        {
            if (!this.CanAdvance(4)) goto Fail;
            value = BigEndianBitConverter.ToInt32(this.buffer, this.UncheckedAdvance(4));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.UInt16"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadUInt16(out ushort value)
        {
            if (!this.CanAdvance(2)) goto Fail;
            value = BigEndianBitConverter.ToUInt16(this.buffer, this.UncheckedAdvance(2));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.Int16"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadInt16(out short value)
        {
            if (!this.CanAdvance(2)) goto Fail;
            value = BigEndianBitConverter.ToInt16(this.buffer, this.UncheckedAdvance(2));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.Int64"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadInt64(out long value)
        {
            if (!this.CanAdvance(8)) goto Fail;
            value = BigEndianBitConverter.ToInt64(this.buffer, this.UncheckedAdvance(8));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a <see cref="System.UInt64"/> from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="value"/> is set to 0.</remarks>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadUInt64(out ulong value)
        {
            if (!this.CanAdvance(8)) goto Fail;
            value = BigEndianBitConverter.ToUInt64(this.buffer, this.UncheckedAdvance(8));
            return true;

        Fail:
            value = 0;
            return false;
        }

        /// <summary>Attempts to read a string with a provided length from the stream.</summary>
        /// <remarks>If the read was unsuccessful, <paramref name="result"/> is set to <see cref="String.Empty"/>.</remarks>
        /// <param name="result">A variable to hold the string.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadLengthString(out string result)
        {
            short length;
            if (!this.TryReadInt16(out length) ||
                !this.CanAdvance(length))
            {
                goto Fail;
            }
            result = this.ReadString(this.UncheckedAdvance(length), length);
            return true;

        Fail:
            result = String.Empty;
            return false;
        }

        /// <summary>
        /// Attempts to read a null-terminated padded string from the stream.
        /// </summary>
        /// <remarks>
        /// If the read was unsuccessful, <paramref name="result"/> is set to <see cref="String.Empty"/>.
        /// Otherwise, <paramref name="result"/> contains the string read from the stream, 
        /// and the stream is advanced <paramref name="length"/> positions.
        /// </remarks>
        /// <param name="length">The length of the padded string segment.</param>
        /// <param name="result">A variable to hold the string.</param>
        /// <returns>true if the read was successful; otherwise, false.</returns>
        public bool TryReadPaddedString(int length, out string result)
        {
            if (!this.CanAdvance(length)) goto Fail;
            result = this.ReadNullTerminatedString(this.UncheckedAdvance(length), length);
            return true;

        Fail:
            result = String.Empty;
            return false;
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

        public void SkipTo(int offset)
        {
            
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

        /// <summary>Reads a <see cref="System.UInt32"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public uint ReadUInt32()
        {
            return BigEndianBitConverter.ToUInt32(this.buffer, this.CheckedAdvance(4));
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

        /// <summary>Reads a <see cref="System.Int64"/> from the stream.</summary>
        /// <returns>The value that was read from the stream.</returns>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if the position will fall outside the bounds of the underlying array segment.
        /// </exception>
        public ulong ReadUInt64()
        {
            return BigEndianBitConverter.ToUInt64(this.buffer, this.CheckedAdvance(8));
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
            int stringStart = this.CheckedAdvance(length);
            return this.ReadNullTerminatedString(stringStart, length);
        }

        #endregion

        /// <summary>
        /// Returns a byte array of the remaining data in the 
        /// stream and advances to the end of the stream.
        /// </summary>
        /// <returns>An array with the buffer's remaining data.</returns>
        public byte[] ReadFully()
        {
            byte[] remainingBytes = new byte[this.Remaining];
            Buffer.BlockCopy(this.buffer, this.UncheckedAdvance(this.Remaining), remainingBytes, 0, this.Remaining);
            return remainingBytes;
        }
    }
}