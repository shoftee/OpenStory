using System;

namespace OpenStory.Networking
{
    /// <summary>
    /// Provides access to properties related to an DataArrived event.
    /// </summary>
    public sealed class DataArrivedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the newly arrived data.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data encapsulated in this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="data" /> is <see langword="null"/>.
        /// </exception>
        internal DataArrivedEventArgs(byte[] data)
        {
            Guard.NotNull(() => data, data);

            Data = data;
        }
    }
}
