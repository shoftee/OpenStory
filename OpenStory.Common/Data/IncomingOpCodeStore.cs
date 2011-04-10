using System.Collections.Generic;

namespace OpenStory.Common.Data
{
    /// <summary>
    /// Represents a lookup table for incoming packet op codes.
    /// </summary>
    public class IncomingOpCodeStore
    {
        private Dictionary<ushort, string> opCodeNames;

        /// <summary>
        /// Initializes a new instance of OutgoingOpCodeStore.
        /// </summary>
        public IncomingOpCodeStore()
        {
            this.opCodeNames = new Dictionary<ushort, string>(256);
        }

        /// <summary>
        /// Attempts to get the label by its packet code.
        /// </summary>
        /// <param name="label">The label for the packet code.</param>
        /// <param name="value">The variable to hold the result.</param>
        /// <returns>true if there was a packet code for the label; otherwise, false.</returns>
        public bool TryGetLabel(ushort value, out string label)
        {
            return this.opCodeNames.TryGetValue(value, out label);
        }
    }
}