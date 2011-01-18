using System;
using System.IO;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a byte buffer with a maximum capacity.
    /// </summary>
    public sealed class BoundedBuffer : IDisposable
    {
        private MemoryStream memoryStream;

        /// <summary>
        /// The number of free bytes at the end of this buffer.
        /// </summary>
        public int FreeSpace { get; private set; }

        /// <summary>
        /// Initializes a new instance of BoundedBuffer with no capacity.
        /// </summary>
        /// <remarks>
        /// A BoundedBuffer with no capacity is unusable, 
        /// any consumer of this class must call <see cref="Reset(System.Int32)"/> 
        /// to assign a buffer capacity before they can use it.</remarks>
        public BoundedBuffer()
        {
            this.FreeSpace = 0;
        }

        /// <summary>
        /// Initializes a new instance of BoundedBuffer with a maximum capacity.
        /// </summary>
        /// <param name="capacity">The maximum capacity to assign.</param>
        public BoundedBuffer(int capacity)
        {
            this.Reset(capacity);
        }

        /// <summary>
        /// Appends a number of bytes to the BoundedBuffer 
        /// starting from the beginning of an array.
        /// </summary>
        /// <remarks>
        /// If this method returns true, <see cref="Extract()"/> or 
        /// <see cref="ExtractAndReset(System.Int32)"/> should be called to 
        /// get the data from the buffer.
        /// </remarks>
        /// <param name="buffer">The array to read bytes from.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the given array segment 
        /// has more bytes than the BoundedBuffer expects 
        /// (which is denoted by the <see cref="FreeSpace"/> property).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer"/> is <c>null</c>.
        /// </exception>
        /// <returns>true if the buffer is full after the append operation; otherwise, false.</returns>
        public bool Append(byte[] buffer, int count)
        {
            return this.Append(buffer, 0, count);
        }

        /// <summary>
        /// Appends a number of bytes to the BoundedBuffer 
        /// starting from a given offset in an array.
        /// </summary>
        /// <remarks>
        /// If this method returns true, <see cref="Extract()"/> or 
        /// <see cref="ExtractAndReset(System.Int32)"/> should be called to 
        /// get the data from the buffer.
        /// </remarks>
        /// <param name="buffer">The array to read bytes from.</param>
        /// <param name="offset">The offset at which to start reading.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the given array segment 
        /// has more bytes than the BoundedBuffer can store.
        /// (which is denoted by the <see cref="FreeSpace"/> property).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if the array segment given by the 
        /// <paramref name="offset"/> and <paramref 
        /// name="count"/> parameters falls outside 
        /// of the given array's bounds.
        /// </exception>
        /// <returns>true if the buffer is full after the append operation; otherwise, false.</returns>
        public bool Append(byte[] buffer, int offset, int count)
        {
            if (this.FreeSpace < count)
                throw new InvalidOperationException("The given segment is too large to store.");

            if (buffer == null) 
                throw new ArgumentNullException("buffer");

            if (offset < 0 || buffer.Length < offset ||
                count <= 0 || offset + count > buffer.Length)
            {
                throw ArraySegmentException.GetByStartAndLength(offset, count);
            }

            this.AppendInternal(buffer, offset, count);
            return this.FreeSpace == 0;
        }

        /// <summary>
        /// Takes bytes from the start of an array and 
        /// appends as much as possible to the BoundedBuffer.
        /// </summary>
        /// <param name="buffer">The array to read from.</param>
        /// <param name="count">The number of bytes to append.</param>
        /// <returns>
        /// The number of bytes that were stored. If there was 
        /// enough space, this is equal to <paramref name="count"/>.
        /// </returns>
        public int AppendFill(byte[] buffer, int count)
        {
            return this.AppendFill(buffer, 0, count);
        }

        /// <summary>
        /// Takes bytes from the start of an array segment and 
        /// appends as much as possible to the BoundedBuffer.
        /// </summary>
        /// <param name="buffer">The array to read from.</param>
        /// <param name="offset">The start of the segment.</param>
        /// <param name="count">The number of bytes to append.</param>
        /// <returns>
        /// The number of bytes that were stored. If there was 
        /// enough space, this is equal to <paramref name="count"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer" /> is <c>null</c>.</exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if the array segment given by the 
        /// <paramref name="offset"/> and <paramref 
        /// name="count"/> parameters falls outside 
        /// of the given array's bounds.
        /// </exception>
        public int AppendFill(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            if (offset < 0 || buffer.Length < offset ||
                count <= 0 || offset + count > buffer.Length)
            {
                throw ArraySegmentException.GetByStartAndLength(offset, count);
            }

            int actualCount = Math.Min(this.FreeSpace, count);
            this.AppendInternal(buffer, offset, actualCount);
            return actualCount;
        }

        private void AppendInternal(byte[] buffer, int offset, int count)
        {
            this.memoryStream.Write(buffer, offset, count);
            this.FreeSpace -= count;
        }

        /// <summary>
        /// Extracts the data from the BoundedBuffer.
        /// </summary>
        /// <remarks>
        /// After calling this method, <see cref="Reset(System.Int32)"/> 
        /// should be called to prepare the buffer for the next segment.
        /// </remarks>
        /// <returns>A byte array of the data in the BoundedBuffer.</returns>
        public byte[] Extract()
        {
            return this.memoryStream.GetBuffer();
        }

        /// <summary>
        /// Prepares the BoundedBuffer for new data.
        /// </summary>
        /// <param name="newCapacity">The new capacity for the BoundedBuffer.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="newCapacity"/> is negative.
        /// </exception>
        public void Reset(int newCapacity)
        {
            if (newCapacity < 0)
                throw new ArgumentOutOfRangeException("newCapacity", newCapacity, "'newCapacity' must be a non-negative number.");
            
            this.memoryStream = new MemoryStream(newCapacity);
            this.FreeSpace = newCapacity;
        }

        /// <summary>
        /// Extracts the data from the BoundedBuffer and prepares it for the new data.
        /// </summary>
        /// <remarks>
        /// Calling this method is equivalent to calling <see cref="Extract()"/> and then
        /// <see cref="Reset(System.Int32)"/> with the same parameter.
        /// </remarks>
        /// <param name="newCapacity">The new capacity for the BoundedBuffer.</param>
        /// <returns>A byte array of the data in the BoundedBuffer.</returns>
        public byte[] ExtractAndReset(int newCapacity)
        {
            byte[] data = this.memoryStream.GetBuffer();

            this.memoryStream = new MemoryStream(newCapacity);
            this.FreeSpace = newCapacity;

            return data;
        }

        private static ArgumentException GetSegmentOutOfBoundsException()
        {
            return new ArgumentException("The array bounds cannot contain the given segment.");
        }

        private static ArgumentOutOfRangeException GetCountMustBePositiveException(int count)
        {
            return new ArgumentOutOfRangeException("count", count, "'count' must be a positive number.");
        }

        private static ArgumentOutOfRangeException GetOffsetOutOfBoundsException(int offset)
        {
            return new ArgumentOutOfRangeException("offset", offset, "'offset' falls outside of the array bounds.");
        }

        /// <summary>
        /// Disposes of the underlying <see cref="MemoryStream"/>.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.memoryStream.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}