using System;
using System.IO;
using OpenStory.Common.Tools;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Represents a moderate-performance byte buffer with a maximum capacity.
    /// </summary>
    public sealed class BoundedBuffer : IDisposable
    {
        private bool isDisposed;

        private MemoryStream stream;
        private int freeSpace;

        /// <summary>
        /// Gets the number of remaining useable bytes in the buffer.
        /// </summary>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        public int FreeSpace
        {
            get
            {
                this.ThrowIfDisposed();
                return this.freeSpace;
            }
        }

        /// <summary>
        /// Initializes a new instance of the  class with no capacity.
        /// </summary>
        /// <remarks>
        /// A <see cref="BoundedBuffer"/> with no capacity is unusable. Any consumer of this class must call <see cref="Reset(int)"/> to assign a capacity before they can use it.
        /// </remarks>
        public BoundedBuffer()
        {
            this.freeSpace = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedBuffer"/> class with a maximum capacity.
        /// </summary>
        /// <param name="capacity">The maximum capacity to assign.</param>
        /// <exception cref="ArgumentOutOfRangeException">The exception is thrown if <paramref name="capacity"/> is non-positive.</exception>
        public BoundedBuffer(int capacity)
        {
            if (capacity <= 0)
            {
                throw GetCapacityIsNonPositiveException(capacity);
            }

            this.ResetInternal(capacity);
        }

        /// <summary>
        /// Takes bytes starting from an offset in an array segment and appends as many as possible to the buffer.
        /// </summary>
        /// <remarks>
        /// This method will append a maximum of <paramref name="count"/> elements to the end of the buffer, starting at <paramref name="offset"/> in <paramref name="buffer"/>.
        /// <para>If there is not enough space for <paramref name="count"/> elements, it will fill the remaining space with bytes from the start of <paramref name="buffer"/>.</para>
        /// </remarks>
        /// <param name="buffer">The array to read from.</param>
        /// <param name="offset">The start of the segment.</param>
        /// <param name="count">The number of bytes to append.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="buffer" /> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> is negative or <paramref name="count"/> is non-positive.</exception>
        /// <exception cref="ArraySegmentException">Thrown if the array segment given by the <paramref name="offset"/> and <paramref name="count"/> parameters falls outside of the given array's bounds.</exception>
        /// <returns>the number of bytes that were stored.</returns>
        public int AppendFill(byte[] buffer, int offset, int count)
        {
            this.ThrowIfDisposed();

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0)
            {
                throw GetOffsetIsNegativeException(offset);
            }

            if (count <= 0)
            {
                throw GetCountIsNonPositiveException(count);
            }

            if (buffer.Length < offset || offset + count > buffer.Length)
            {
                throw ArraySegmentException.GetByStartAndLength(offset, count);
            }

            int stored = this.AppendInternal(buffer, offset, count);
            return stored;
        }

        private int AppendInternal(byte[] buffer, int offset, int requested)
        {
            int stored = Math.Min(this.FreeSpace, requested);

            this.stream.Write(buffer, offset, stored);
            this.freeSpace -= stored;

            return stored;
        }

        /// <summary>
        /// Prepares the buffer for new data.
        /// </summary>
        /// <param name="newCapacity">The new capacity for the buffer.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="newCapacity"/> is negative.</exception>
        public void Reset(int newCapacity)
        {
            this.ThrowIfDisposed();

            if (newCapacity < 0)
            {
                throw GetNewCapacityIsNegativeException(newCapacity);
            }

            this.ResetInternal(newCapacity);
        }

        /// <summary>
        /// Extracts the data from the <see cref="BoundedBuffer"/> and prepares it for the new data.
        /// </summary>
        /// <param name="newCapacity">The new capacity for the <see cref="BoundedBuffer"/>.</param>
        /// <inheritdoc cref="ThrowIfDisposed()" select="exception[@cref='ObjectDisposedException']" />
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="newCapacity"/> is negative.</exception>
        /// <returns>the data that was in the <see cref="BoundedBuffer"/>.</returns>
        public byte[] ExtractAndReset(int newCapacity)
        {
            this.ThrowIfDisposed();

            if (newCapacity < 0)
            {
                throw GetNewCapacityIsNegativeException(newCapacity);
            }

            byte[] data = Arrays<byte>.Empty;
            if (this.stream != null)
            {
                data = this.stream.GetBuffer();
            }

            this.ResetInternal(newCapacity);

            return data;
        }

        private void ResetInternal(int newCapacity)
        {
            this.stream = new MemoryStream(newCapacity);
            this.freeSpace = newCapacity;
        }

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Virtual dispose method. When overriding, call the base implementation before your logic.
        /// </summary>
        /// <param name="disposing">
        /// A parameter to denote whether the method is being called during Disposal or Finalization.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!this.isDisposed && disposing)
            {
                var copy = this.stream;
                if (copy != null)
                {
                    copy.Dispose();
                    this.stream = null;
                }

                this.isDisposed = true;
            }
        }

        #endregion

        #region Exception helpers

        /// <summary>
        /// Throws a new <see cref="ObjectDisposedException"/> if the current object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// Thrown if the <see cref="BoundedBuffer"/> has been disposed.
        /// </exception>
        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        private static ArgumentOutOfRangeException GetCapacityIsNonPositiveException(int capacity)
        {
            return new ArgumentOutOfRangeException("capacity", capacity, Exceptions.CapacityMustBePositive);
        }

        private static ArgumentOutOfRangeException GetNewCapacityIsNegativeException(int newCapacity)
        {
            return new ArgumentOutOfRangeException("newCapacity", newCapacity, Exceptions.CapacityMustBeNonNegative);
        }

        private static ArgumentOutOfRangeException GetCountIsNonPositiveException(int count)
        {
            return new ArgumentOutOfRangeException("count", count, Exceptions.CountMustBePositive);
        }

        private static ArgumentOutOfRangeException GetOffsetIsNegativeException(int offset)
        {
            return new ArgumentOutOfRangeException("offset", offset, Exceptions.OffsetMustBeNonNegative);
        }

        #endregion
    }
}
