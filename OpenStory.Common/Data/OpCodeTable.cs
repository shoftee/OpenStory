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
        private Dictionary<ushort, string> incomingTable;
        private Dictionary<string, ushort> outgoingTable;

        /// <summary>
        /// Initializes a new instance of OpCodeTable.
        /// </summary>
        protected OpCodeTable()
        {
            this.incomingTable = new Dictionary<ushort, string>(256);
            this.outgoingTable = new Dictionary<string, ushort>(256);
        }

        /// <summary>
        /// Loads the op code information for this OpCodeTable instance.
        /// </summary>
        public abstract void LoadOpCodes();

        /// <summary>
        /// Attempts to get the label for a specified incoming packet code.
        /// </summary>
        /// <param name="opCode">The packet code to look up the label of.</param>
        /// <param name="label">A variable to hold the result.</param>
        /// <returns>true if there was an incoming packet code for the label; otherwise, false.</returns>
        public bool TryGetIncomingLabel(ushort opCode, out string label)
        {
            return this.incomingTable.TryGetValue(opCode, out label);
        }

        /// <summary>
        /// Attempts to get the outgoing packet code for a specified label.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="opCode">The variable to hold the result.</param>
        /// <returns>true if there was an outgoing packet with the label; otherwise, false.</returns>
        public bool TryGetOutgoingOpCode(string label, out ushort opCode)
        {
            return this.outgoingTable.TryGetValue(label, out opCode);
        }

        /// <summary>
        /// Adds an entry to the outgoing packet information list.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="opCode">The op code of the outgoing packet.</param>
        /// <returns>true if the operation was successful; otherwise, false.</returns>
        protected bool AddOutgoing(string label, ushort opCode)
        {
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
        /// <returns>true if the operation was successful; otherwise, false.</returns>
        protected bool AddIncoming(ushort opCode, string label)
        {
            if (this.incomingTable.ContainsKey(opCode))
            {
                return false;
            }

            this.incomingTable.Add(opCode, label);
            return true;
        }

        /// <summary>
        /// Provides a <see cref="PacketBuilder"/> with a pre-added op code for the specified packet label.
        /// </summary>
        /// <param name="label">The label for the packet op code.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="label"/> has no corresponding known packet op code.
        /// </exception>
        /// <returns>The constructed <see cref="PacketBuilder"/> instance.</returns>
        public PacketBuilder NewPacket(string label)
        {
            ushort opCode;
            if (!this.outgoingTable.TryGetValue(label, out opCode))
            {
                throw new ArgumentException("The given label has no corresponding op code.", "label");
            }
            PacketBuilder builder = new PacketBuilder();
            builder.WriteInt16(opCode);
            return builder;
        }
    }
}
