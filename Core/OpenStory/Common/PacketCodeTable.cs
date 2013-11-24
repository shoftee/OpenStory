using System;
using System.Collections.Generic;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a lookup table for packet op codes and labels.
    /// </summary>
    public abstract class PacketCodeTable : IPacketCodeTable
    {
        private readonly Dictionary<ushort, string> incomingTable;
        private readonly Dictionary<string, ushort> outgoingTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCodeTable"/> class.
        /// </summary>
        protected PacketCodeTable()
        {
            this.incomingTable = new Dictionary<ushort, string>(256);
            this.outgoingTable = new Dictionary<string, ushort>(256);
        }

        /// <summary>
        /// Loads the op code information for this instance of <see cref="PacketCodeTable"/>.
        /// </summary>
        public void LoadPacketCodes()
        {
            this.incomingTable.Clear();
            this.outgoingTable.Clear();

            this.LoadPacketCodesInternal();
        }

        /// <summary>
        /// This is a hook to the end of the <see cref="LoadPacketCodes"/> method.
        /// </summary>
        protected abstract void LoadPacketCodesInternal();

        /// <inheritdoc />
        public string GetIncomingLabel(ushort code)
        {
            return this.incomingTable[code];
        }

        /// <inheritdoc />
        public bool TryGetIncomingLabel(ushort code, out string label)
        {
            return this.incomingTable.TryGetValue(code, out label);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="label"/> is the empty string.
        /// </exception>
        public ushort GetOutgoingCode(string label)
        {
            Guard.NotNullOrEmpty(() => label, label);

            return this.outgoingTable[label];
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="label"/> is the empty string.
        /// </exception>
        public bool TryGetOutgoingCode(string label, out ushort code)
        {
            Guard.NotNullOrEmpty(() => label, label);

            return this.outgoingTable.TryGetValue(label, out code);
        }

        /// <summary>
        /// Adds an entry to the outgoing packet information list.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="code">The packet code of the outgoing packet.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="label"/> is the empty string.
        /// </exception>
        /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
        protected bool AddOutgoing(string label, ushort code)
        {
            Guard.NotNullOrEmpty(() => label, label);

            if (this.outgoingTable.ContainsKey(label))
            {
                return false;
            }

            this.outgoingTable.Add(label, code);
            return true;
        }

        /// <summary>
        /// Adds an entry to the incoming packet information list.
        /// </summary>
        /// <param name="code">The packet code pf the incoming packet.</param>
        /// <param name="label">The label for the incoming packet.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="label"/> is the empty string.
        /// </exception>
        /// <returns><see langword="true"/> if the operation was successful; otherwise, <see langword="false"/>.</returns>
        protected bool AddIncoming(ushort code, string label)
        {
            Guard.NotNullOrEmpty(() => label, label);

            if (this.incomingTable.ContainsKey(code))
            {
                return false;
            }

            this.incomingTable.Add(code, label);
            return true;
        }
    }
}
