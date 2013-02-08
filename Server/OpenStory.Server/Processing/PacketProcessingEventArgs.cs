using System;
using OpenStory.Common.IO;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Contains event data for a packet that requires processing.
    /// </summary>
    public sealed class PacketProcessingEventArgs : EventArgs
    {
        private readonly PacketReader reader;

        /// <summary>
        /// Gets the label of the packet.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// Gets a fresh reader over the packet's content.
        /// </summary>
        public IUnsafePacketReader Reader
        {
            get { return new PacketReader(this.reader); }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="PacketProcessingEventArgs"/>.
        /// </summary>
        /// <param name="label">The known label of the packet.</param>
        /// <param name="reader">The packet reader, after having read the label code.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="reader"/> is <c>null</c>.
        /// </exception>
        internal PacketProcessingEventArgs(string label, PacketReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this.Label = label;
            this.reader = reader;
        }
    }
}