using System.Collections.Generic;

namespace OpenStory.Common.Data
{
    /// <summary>
    /// Represents a lookup table for outgoing packet op codes.
    /// </summary>
    public class OutgoingOpCodeStore
    {
        private Dictionary<string, ushort> opCodeValues;

        /// <summary>
        /// Initializes a new instance of OutgoingOpCodeStore.
        /// </summary>
        public OutgoingOpCodeStore()
        {
            this.opCodeValues = new Dictionary<string, ushort>(256);
        }

        /// <summary>
        /// Attempts to get the packet code by its label.
        /// </summary>
        /// <param name="label">The label of the packet code.</param>
        /// <param name="value">The variable to hold the result.</param>
        /// <returns>true if there was a packet with the label; otherwise, false.</returns>
        public bool TryGetValue(string label, out ushort value)
        {
            return opCodeValues.TryGetValue(label, out value);
        }
    }
}