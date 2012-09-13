using System;
using OpenStory.Common.IO;

namespace OpenStory.Common.Data
{
    /// <summary>
    /// Provides methods for looking up packet labels and op codes.
    /// </summary>
    public interface IOpCodeTable
    {
        /// <summary>
        /// Attempts to get the label for a specified incoming packet code.
        /// </summary>
        /// <param name="opCode">The packet code to look up the label of.</param>
        /// <param name="label">A variable to hold the result.</param>
        /// <returns><c>true</c> if there was an incoming packet code for the label; otherwise, <c>false</c>.</returns>
        bool TryGetIncomingLabel(ushort opCode, out string label);

        /// <summary>
        /// Attempts to get the outgoing packet code for a specified label.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="opCode">The variable to hold the result.</param>
        /// <returns><c>true</c> if there was an outgoing packet with the label; otherwise, <c>false</c>.</returns>
        bool TryGetOutgoingOpCode(string label, out ushort opCode);
    }
}
