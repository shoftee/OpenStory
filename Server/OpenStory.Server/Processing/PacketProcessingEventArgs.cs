using System;
using OpenStory.Common.IO;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Contains event data for a packet that requires processing.
    /// </summary>
    public sealed class PacketProcessingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the label of the packet.
        /// </summary>
        public string Label { get; private set; }

        private readonly PacketReader reader;
        /// <summary>
        /// Gets a fresh reader over the packet's content.
        /// </summary>
        public PacketReader Reader
        {
            get { return new PacketReader(this.reader); }
        }

        internal PacketProcessingEventArgs(string label, PacketReader reader)
        {
            this.Label = label;
            this.reader = reader;
        }
    }
}