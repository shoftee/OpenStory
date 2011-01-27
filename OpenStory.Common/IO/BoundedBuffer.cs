using System;
using System.IO;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a moderate-performance byte buffer with a maximum capacity.
    /// </summary>
    public sealed class BoundedBuffer : IDisposable
    {
        private MemoryStream memoryStream;

        /// <summary>
        /// Gets the number of useable bytes at the end of the BoundedBuffer.
        /// </summary>
        public int FreeSpace { get; private set; }

        /// <summary>
        /// Initializes a new instance of the 
        /// BoundedBuffer class with no capacity.
        /// </summary>
        /// <remarks>
        /// A BoundedBuffer with no capacity is unusable. 
        /// Any consumer of this class must call <see cref="Reset(int)"/> 
        /// to assign a capacity before they can use it.
        /// </remarks>
        public BoundedBuffer()
        {
            this.FreeSpace = 0;
        }

        /// <summary>
        /// Initializes a new instance of BoundedBuffer with a maximum capacity.
        /// </summary>
        /// <param name="capacity">The maximum capacity to assign.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The exception is thrown if <paramref name="capacity"/> 
        /// is non-positive.
        /// </exception>
        public BoundedBuffer(int capacity)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity", capacity, "'capacity' must be a positive integer. Call the default constructor instead.");
            }

            this.Reset(capacity);
        }

        /// <summary>
        /// Takes bytes from the start of an array and 
        /// appends as many as possible to the BoundedBuffer.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method will append a maximum of <paramref name="count"/> elements from the 
        /// start of <paramref name="buffer"/> to the end of the BoundedBuffer.
        /// If there is not enough space for <c>count</c> elements, 
        /// it will fill the remaining BoundedBuffer space with bytes from the start of
        /// <c>buffer</c>.
        /// </para><para>
        /// Calling this method is equivalent to calling 
        /// <see cref="AppendFill(byte[],int,int)"/> with offset set to 0.
        /// </para>
        /// </remarks>
        /// <param name="buffer">The array to read from.</param>
        /// <param name="count">The number of bytes to append.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if <paramref name="buffer"/> has 
        /// less than <paramref name="count"/> elements.
        /// </exception>
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
        /// <remarks>
        /// This method will append a maximum of <paramref name="count"/> elements 
        /// to the end of the BoundedBuffer, starting at 
        /// <paramref name="offset"/> in <paramref name="buffer"/> .
        /// </remarks>
        /// <param name="buffer">The array to read from.</param>
        /// <param name="offset">The start of the segment.</param>
        /// <param name="count">The number of bytes to append.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="buffer" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArraySegmentException">
        /// Thrown if the array segment given by the 
        /// <paramref name="offset"/> and <paramref 
        /// name="count"/> parameters falls outside 
        /// of the given array's bounds.
        /// </exception>
        /// <returns>
        /// The number of bytes that were stored. If there was 
        /// enough space, this is equal to <paramref name="count"/>.
        /// </returns>
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
        /// After calling this method, <see cref="Reset(int)"/> 
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
        /// <see cref="Reset(int)"/> with the same parameter.
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