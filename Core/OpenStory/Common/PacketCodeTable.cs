using System;
using System.Collections.Generic;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a lookup table for packet op codes and labels.
    /// </summary>
    public abstract class PacketCodeTable : IPacketCodeTable
    {
        private readonly Dictionary<ushort, string> _incomingTable;
        private readonly Dictionary<string, ushort> _outgoingTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCodeTable"/> class.
        /// </summary>
        protected PacketCodeTable()
        {
            _incomingTable = new Dictionary<ushort, string>(256);
            _outgoingTable = new Dictionary<string, ushort>(256);
        }

        /// <summary>
        /// Loads the op code information for this instance of <see cref="PacketCodeTable"/>.
        /// </summary>
        public void LoadPacketCodes()
        {
            _incomingTable.Clear();
            _outgoingTable.Clear();

            LoadPacketCodesInternal();
        }

        /// <summary>
        /// This is a hook to the end of the <see cref="LoadPacketCodes"/> method.
        /// </summary>
        protected abstract void LoadPacketCodesInternal();

        /// <inheritdoc />
        public string GetIncomingLabel(ushort code)
        {
            return _incomingTable[code];
        }

        /// <inheritdoc />
        public bool TryGetIncomingLabel(ushort code, out string label)
        {
            return _incomingTable.TryGetValue(code, out label);
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

            return _outgoingTable[label];
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

            return _outgoingTable.TryGetValue(label, out code);
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

            if (_outgoingTable.ContainsKey(label))
            {
                return false;
            }

            _outgoingTable.Add(label, code);
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

            if (_incomingTable.ContainsKey(code))
            {
                return false;
            }

            _incomingTable.Add(code, label);
            return true;
        }
    }
}
