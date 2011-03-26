using System;
using System.IO;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a class for constructing packets.
    /// </summary>
    public sealed class PacketBuilder : IDisposable
    {
        private bool isDisposed;
        private MemoryStream stream;

        /// <summary>Initializes a new PacketBuilder instance with the default capacity.</summary>
        public PacketBuilder()
        {
            this.stream = new MemoryStream();
        }

        /// <summary>Initializes a new PacketBuilder instance.</summary>
        /// <param name="capacity">The initial capacity for the underlying stream.</param>
        public PacketBuilder(int capacity)
        {
            this.stream = new MemoryStream(capacity);
        }

        #region IDisposable Members

        /// <summary>Disposes of the underlying stream.</summary>
        /// <remarks>Calling any instance methods after this will cause them to throw an ObjectDisposedException.</remarks>
        public void Dispose()
        {
            if (this.isDisposed) return;
            this.isDisposed = true;
            this.stream.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>Writes a <see cref="System.Int64"/> to the end of the packet.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteLong(long value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.UInt64"/> to the end of the packet.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteLong(ulong value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.Int32"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteInt(int value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.UInt32"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteInt(uint value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.Int16"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteShort(short value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.UInt16"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteShort(ushort value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a <see cref="System.Byte"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteByte(byte value)
        {
            this.CheckDisposed();
            this.stream.WriteByte(value);
        }

        /// <summary>Writes an array of bytes to the stream.</summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteBytes(byte[] bytes)
        {
            this.CheckDisposed();
            this.stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>Writes a <see cref="System.Boolean"/> to the stream.</summary>
        /// <param name="value">The value to write.</param>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteBool(bool value)
        {
            this.CheckDisposed();
            this.WriteDirect(BigEndianBitConverter.GetBytes(value));
        }

        /// <summary>Writes a string and its length to the stream.</summary>
        /// <remarks>The length of the stream is written first.</remarks>
        /// <param name="value">The string to write.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WriteLengthString(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            this.WriteShort((short) value.Length);
            this.WriteDirect(Encoding.UTF8.GetBytes(value));
        }

        /// <summary>Writes a padded string to the stream.</summary>
        /// <param name="value">The string to write.</param>
        /// <param name="padLength">The length to pad the string to.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown 
        /// if <paramref name="padLength"/> is a non-positive number, 
        /// OR, 
        /// if <paramref name="value"/> is not shorter than padLength.
        /// </exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">Thrown if the PacketBuilder has been disposed.</exception>
        public void WritePaddedString(string value, int padLength)
        {
            this.CheckDisposed();
            if (value == null) throw new ArgumentNullException("value");
            if (padLength <= 0)
                throw new ArgumentOutOfRangeException("padLength", padLength,
                                                      "The pad length must be a positive number.");
            if (value.Length >= padLength)
                throw new ArgumentOutOfRangeException("value", "The string is not shorter than the pad length.");

            var stringBytes = new byte[padLength];
            Encoding.UTF8.GetBytes(value, 0, value.Length, stringBytes, 0);
            stringBytes[value.Length] = 0;
            this.WriteDirect(stringBytes);
        }

        private void WriteDirect(byte[] bytes)
        {
            int length = bytes.Length;
            this.stream.Write(bytes, 0, length);
        }

        private void CheckDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException("PacketWriter");
            }
        }

        /// <summary>
        /// Gets a copy of the internal byte buffer of the PacketBuilder.
        /// </summary>
        /// <returns>The copy of the byte buffer.</returns>
        public byte[] ToByteArray()
        {
            byte[] buffer = this.stream.GetBuffer();
            int length = (int) this.stream.Position;
            var array = new byte[length];
            Buffer.BlockCopy(buffer, 0, array, 0, length);
            return array;
        }
    }
}