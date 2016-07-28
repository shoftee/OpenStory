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
        public PacketReader Reader => new PacketReader(this.buffer);

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="packet">The packet data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <see langword="null"/>.
        /// </exception>
        public PacketReceivedEventArgs(byte[] packet)
        {
            Guard.NotNull(() => packet, packet);

            this.buffer = packet;
        }
    }
}
