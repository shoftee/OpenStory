using System;

namespace OpenStory.Networking
{
    /// <summary>
    /// Provides access to properties related to an OnDataArrived event.
    /// </summary>
    public class DataArrivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataArrivedEventArgs"/> class.
        /// </summary>
        /// <param name="data">The data encapsulated in this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data" /> is <c>null</c>.</exception>
        internal DataArrivedEventArgs(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            this.Data = data;
        }

        /// <summary>
        /// Gets the newly arrived data.
        /// </summary>
        public byte[] Data { get; private set; }
    }
}