using System;
using System.Collections.Generic;
using OpenStory.Common.IO;

namespace OpenStory.Common.Data
{
    /// <summary>
    /// Represents a lookup table for packet op codes and labels.
    /// </summary>
    public abstract class OpCodeTable : IOpCodeTable
    {
        private readonly Dictionary<ushort, string> incomingTable;
        private readonly Dictionary<string, ushort> outgoingTable;

        /// <summary>
        /// Initializes a new instance of <see cref="OpCodeTable"/>.
        /// </summary>
        protected OpCodeTable()
        {
            this.incomingTable = new Dictionary<ushort, string>(256);
            this.outgoingTable = new Dictionary<string, ushort>(256);
        }

        /// <summary>
        /// Loads the op code information for this instance of <see cref="OpCodeTable"/>.
        /// </summary>
        public void LoadOpCodes()
        {
            this.incomingTable.Clear();
            this.outgoingTable.Clear();

            this.LoadOpCodesInternal();
        }

        /// <summary>
        /// This is a hook to the end of the <see cref="LoadOpCodes"/> method.
        /// </summary>
        protected abstract void LoadOpCodesInternal();

        /// <inheritdoc />
        public bool TryGetIncomingLabel(ushort opCode, out string label)
        {
            return this.incomingTable.TryGetValue(opCode, out label);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <c>null</c>.
        /// </exception>
        public bool TryGetOutgoingOpCode(string label, out ushort opCode)
        {
            if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            return this.outgoingTable.TryGetValue(label, out opCode);
        }

        /// <summary>
        /// Adds an entry to the outgoing packet information list.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="opCode">The op code of the outgoing packet.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <c>null</c>.
        /// </exception>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        protected bool AddOutgoing(string label, ushort opCode)
        {
            if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            if (this.outgoingTable.ContainsKey(label))
            {
                return false;
            }

            this.outgoingTable.Add(label, opCode);
            return true;
        }

        /// <summary>
        /// Adds an entry to the incoming packet information list.
        /// </summary>
        /// <param name="opCode">The op code pf the incoming packet.</param>
        /// <param name="label">The label for the incoming packet.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="label"/> is <c>null</c>.
        /// </exception>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        protected bool AddIncoming(ushort opCode, string label)
        {
            if (label == null)
            {
                throw new ArgumentNullException("label");
            }
            
            if (this.incomingTable.ContainsKey(opCode))
            {
                return false;
            }

            this.incomingTable.Add(opCode, label);
            return true;
        }

        /// <inheritdoc />
        public PacketBuilder NewPacket(string label)
        {
            if (label == null)
            {
                throw new ArgumentNullException("label");
            }

            ushort opCode;
            if (!this.outgoingTable.TryGetValue(label, out opCode))
            {
                throw new ArgumentException("The given label has no corresponding op code.", "label");
            }

            var builder = new PacketBuilder();
            builder.WriteInt16(opCode);
            return builder;
        }
    }
}
