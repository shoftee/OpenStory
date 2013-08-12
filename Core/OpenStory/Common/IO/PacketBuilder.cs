using System;
using System.IO;
using System.Text;
using OpenStory.Common.Tools;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a class for constructing game packets.
    /// </summary>
    /// <remarks>
    /// This class exclusively uses little-endian byte order.
    /// </remarks>
    public sealed class PacketBuilder : IPacketBuilder, IDisposable
    {
        private bool isDisposed;

        private MemoryStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuilder"/> class with the default capacity.
        /// </summary>
        public PacketBuilder()
        {
            this.stream = new MemoryStream();
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt64(long number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt64(ulong number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(int number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(uint number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(short number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(ushort number)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteByte(byte number)
        {
            this.ThrowIfDisposed();

            this.stream.WriteByte(number);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteZeroes(int count)
        {
            this.ThrowIfDisposed();

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, Exceptions.CountMustBeNonNegative);
            }

            for (int i = 0; i < count; i++)
            {
                this.stream.WriteByte(0);
            }
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteBytes(byte[] bytes)
        {
            this.ThrowIfDisposed();

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteBoolean(bool boolean)
        {
            this.ThrowIfDisposed();

            this.WriteDirect(LittleEndianBitConverter.GetBytes(boolean));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteLengthString(string @string)
        {
            this.ThrowIfDisposed();

            if (@string == null)
            {
                throw new ArgumentNullException("string");
            }

            this.WriteInt16((short)@string.Length);
            if (@string.Length > 0)
            {
                this.WriteDirect(Encoding.UTF8.GetBytes(@string));
            }
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WritePaddedString(string @string, int paddingLength)
        {
            this.ThrowIfDisposed();

            if (@string == null)
            {
                throw new ArgumentNullException("string");
            }

            if (paddingLength <= 0)
            {
                throw new ArgumentOutOfRangeException("paddingLength", paddingLength, Exceptions.PaddingLengthMustBePositive);
            }

            if (@string.Length > paddingLength - 1)
            {
                throw new ArgumentException(Exceptions.StringMustBeShorterThanPaddingLength);
            }

            var stringBytes = new byte[paddingLength];
            Encoding.UTF8.GetBytes(@string, 0, @string.Length, stringBytes, 0);
            stringBytes[@string.Length] = 0;

            this.WriteDirect(stringBytes);
        }

        /// <summary>
        /// Gets a copy of the internal byte buffer of the PacketBuilder.
        /// </summary>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <returns>the copy of the byte buffer.</returns>
        public byte[] ToByteArray()
        {
            this.ThrowIfDisposed();

            byte[] buffer = this.stream.GetBuffer();
            var length = (int)this.stream.Position;
            var array = buffer.CopySegment(0, length);
            return array;
        }

        private void WriteDirect(byte[] bytes)
        {
            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if the current object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the <see cref="PacketBuilder"/> has been disposed.
        /// </exception>
        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
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

            var localStream = this.stream;
            if (localStream != null)
            {
                localStream.Dispose();
                this.stream = null;
            }

            this.isDisposed = true;
        }

        #endregion
    }
}
