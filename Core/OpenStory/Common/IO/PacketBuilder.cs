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
    public sealed class PacketBuilder : IPacketBuilder, IDisposable
    {
        private bool _isDisposed;

        private MemoryStream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuilder"/> class with the default capacity.
        /// </summary>
        public PacketBuilder()
        {
            _stream = new MemoryStream();
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt64(long number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt64(ulong number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(int number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt32(uint number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(short number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(ushort number)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(number));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(int number)
        {
            WriteInt16((short)number);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteInt16(uint number)
        {
            WriteInt16((ushort)number);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteByte(byte number)
        {
            ThrowIfDisposed();

            _stream.WriteByte(number);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteByte(int number)
        {
            WriteByte((byte)number);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteZeroes(int count)
        {
            ThrowIfDisposed();

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, CommonStrings.CountMustBePositive);
            }

            for (int i = 0; i < count; i++)
            {
                _stream.WriteByte(0);
            }
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteBytes(byte[] bytes)
        {
            ThrowIfDisposed();

            Guard.NotNull(() => bytes, bytes);

            _stream.Write(bytes, 0, bytes.Length);
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteBoolean(bool boolean)
        {
            ThrowIfDisposed();

            WriteDirect(LittleEndianBitConverter.GetBytes(boolean));
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WriteLengthString(string @string)
        {
            ThrowIfDisposed();

            Guard.NotNull(() => @string, @string);

            WriteInt16((short)@string.Length);
            if (@string.Length > 0)
            {
                WriteDirect(Encoding.UTF8.GetBytes(@string));
            }
        }

        /// <inheritdoc />
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public void WritePaddedString(string @string, int paddingLength)
        {
            ThrowIfDisposed();

            Guard.NotNull(() => @string, @string);

            if (paddingLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paddingLength), paddingLength, CommonStrings.PaddingLengthMustBePositive);
            }

            if (@string.Length > paddingLength - 1)
            {
                throw new ArgumentException(CommonStrings.StringMustBeShorterThanPaddingLength);
            }

            var stringBytes = new byte[paddingLength];
            Encoding.UTF8.GetBytes(@string, 0, @string.Length, stringBytes, 0);
            stringBytes[@string.Length] = 0;

            WriteDirect(stringBytes);
        }

        /// <summary>
        /// Gets a copy of the internal byte buffer of the PacketBuilder.
        /// </summary>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <returns>the copy of the byte buffer.</returns>
        public byte[] ToByteArray()
        {
            ThrowIfDisposed();

            byte[] buffer = _stream.GetBuffer();
            var length = (int)_stream.Position;
            var array = buffer.CopySegment(0, length);
            return array;
        }

        private void WriteDirect(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if the current object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the <see cref="PacketBuilder"/> has been disposed.
        /// </exception>
        private void ThrowIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
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
            if (!_isDisposed)
            {
                Misc.AssignNullAndDispose(ref _stream);

                _isDisposed = true;
            }
        }

        #endregion
    }
}
