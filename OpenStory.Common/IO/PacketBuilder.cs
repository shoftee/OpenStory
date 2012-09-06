using System;
using System.IO;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a class for constructing game packets.
    /// </summary>
    /// <remarks>
    /// This class exclusively uses little-endian byte order.
    /// </remarks>
    public sealed class PacketBuilder : IDisposable
    {
        private bool isDisposed;

        private readonly MemoryStream stream;

        /// <summary>
        /// Initializes a new <see cref="PacketBuilder"/> instance with the default capacity.
        /// </summary>
        public PacketBuilder()
        {
            this.stream = new MemoryStream();
        }

        /// <summary>
        /// Initializes a new <see cref="PacketBuilder"/> instance.
        /// </summary>
        /// <param name="capacity">The initial capacity for the underlying stream.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="capacity"/> is non-positive.
        /// </exception>
        public PacketBuilder(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity", capacity, "'capacity' must be a positive integer.");
            }

            this.stream = new MemoryStream(capacity);
        }

        /// <summary>
        /// Writes a <see cref="System.Int64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteLong(long number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.UInt64"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteLong(ulong number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.Int32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(int number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.UInt32"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(uint number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.Int16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(short number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.UInt16"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(ushort number)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <summary>
        /// Writes a <see cref="System.Byte"/> to the end of the packet.
        /// </summary>
        /// <param name="number">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteByte(byte number)
        {
            this.ThrowIfDisposed();
            this.stream.WriteByte(number);
        }

        /// <summary>
        /// Writes a specified number of 0-value bytes to the end of the packet.
        /// </summary>
        /// <param name="count">The number of bytes to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="count"/> is negative.
        /// </exception>
        public void WriteZeroes(int count)
        {
            this.ThrowIfDisposed();
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", "'count' must be a non-negative integer.");
            }

            for (int i = 0; i < count; i++)
            {
                this.stream.WriteByte(0);
            }
        }

        /// <summary>
        /// Writes an array of bytes to the end of the packet.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="bytes"/> is <c>null</c>.
        /// </exception>
        public void WriteBytes(byte[] bytes)
        {
            this.ThrowIfDisposed();
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes a <see cref="System.Boolean"/> to the end of the packet.
        /// </summary>
        /// <param name="boolean">The value to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteBoolean(bool boolean)
        {
            this.ThrowIfDisposed();
            this.WriteDirect(LittleEndianBitConverter.GetBytes(boolean));
        }

        /// <summary>
        /// Writes a length-prefixed UTF8 string to the end of the packet.
        /// </summary>
        /// <remarks>The length of the stream is written first.</remarks>
        /// <param name="s">The string to write.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="s"/> is <c>null</c>.
        /// </exception>
        public void WriteLengthString(string s)
        {
            this.ThrowIfDisposed();
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            this.WriteInt16((short)s.Length);
            if (s.Length > 0)
            {
                this.WriteDirect(Encoding.UTF8.GetBytes(s));
            }
        }

        /// <summary>
        /// Writes a padded string to the end of the packet.
        /// </summary>
        /// <param name="s">The string to write.</param>
        /// <param name="padLength">The length to pad the string to.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>Thrown if <paramref name="padLength"/> is a non-positive number, </para>
        /// <para>OR,</para> 
        /// <para>if <paramref name="s"/> is longer than <paramref name="padLength"/>.</para>
        /// </exception>
        public void WritePaddedString(string s, int padLength)
        {
            this.ThrowIfDisposed();
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (padLength <= 0)
            {
                throw new ArgumentOutOfRangeException("padLength", padLength,
                                                      "The pad length must be a positive number.");
            }
            if (s.Length > padLength)
            {
                throw new ArgumentOutOfRangeException("s", "The string is longer than the pad length.");
            }

            var stringBytes = new byte[padLength];
            Encoding.UTF8.GetBytes(s, 0, s.Length, stringBytes, 0);
            stringBytes[s.Length] = 0;
            this.WriteDirect(stringBytes);
        }

        /// <summary>
        /// Gets a copy of the internal byte buffer of the PacketBuilder.
        /// </summary>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <returns>the copy of the byte buffer.</returns>
        public byte[] ToByteArray()
        {
            byte[] buffer = this.stream.GetBuffer();
            var length = (int)this.stream.Position;
            var array = new byte[length];
            Buffer.BlockCopy(buffer, 0, array, 0, length);
            return array;
        }

        private void WriteDirect(byte[] bytes)
        {
            int length = bytes.Length;
            this.stream.Write(bytes, 0, length);
        }

        /// <exception cref="ObjectDisposedException">
        /// Thrown if the <see cref="PacketBuilder"/> has been disposed.
        /// </exception>
        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("stream");
            }
        }

        #region IDisposable Members

        /// <inheritdoc />
        /// <remarks>
        /// <inheritdoc />
        /// Calling instance methods after calling this will cause them to throw an <see cref="ObjectDisposedException"/>.
        /// </remarks>
        public void Dispose()
        {
            if (this.isDisposed)
            {
                return;
            }

            this.isDisposed = true;
            if (this.stream != null)
            {
                this.stream.Dispose();
            }
        }

        #endregion
    }
}
