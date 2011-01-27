using System;
using OpenStory.Common.IO;

namespace OpenStory.Networking
{
    /// <summary>
    /// Incoming packet data, EventArgs-style.
    /// </summary>
    public class IncomingPacketEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a <see cref="PacketReader"/> over the packet.
        /// </summary>
        public PacketReader Reader { get; private set; }

        /// <summary>
        /// Initializes a new instance of the IncomingPacketEventArgs class.
        /// </summary>
        /// <param name="packet">The packet data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        internal IncomingPacketEventArgs(byte[] packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");

            this.Reader = new PacketReader(packet);
        }
    }
}