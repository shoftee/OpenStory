using System;
using OpenStory.Common.IO;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a received packet that can be passed with a raised event.
    /// </summary>
    public sealed class PacketReceivedEventArgs : EventArgs
    {
        private readonly byte[] buffer;

        /// <summary>
        /// Gets a new <see cref="PacketReader"/> for the packet.
        /// </summary>
        public PacketReader Reader
        {
            get { return new PacketReader(buffer); }
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="PacketReceivedEventArgs"/>.
        /// </summary>
        /// <param name="packet">The packet data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        public PacketReceivedEventArgs(byte[] packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            this.buffer = packet;
        }
    }
}
