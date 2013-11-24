namespace OpenStory.Common
{
    /// <summary>
    /// Provides methods for looking up packet labels and op codes.
    /// </summary>
    public interface IPacketCodeTable
    {
        /// <summary>
        /// Retrieves the label for the specified incoming packet code.
        /// </summary>
        /// <param name="code">The incoming packet code to get the label for.</param>
        /// <returns>The label for the specified packet code.</returns>
        string GetIncomingLabel(ushort code);

        /// <summary>
        /// Attempts to get the label for a specified incoming packet code.
        /// </summary>
        /// <param name="code">The packet code to look up the label of.</param>
        /// <param name="label">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if there was an incoming packet code for the label; otherwise, <see langword="false"/>.</returns>
        bool TryGetIncomingLabel(ushort code, out string label);

        /// <summary>
        /// Retrieves the packet code for the specified outgoing packet label.
        /// </summary>
        /// <param name="label">The outgoing packet label to get the code for.</param>
        /// <returns>The outgoing packet code for the specified label.</returns>
        ushort GetOutgoingCode(string label);

        /// <summary>
        /// Attempts to get the outgoing packet code for a specified label.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="code">The variable to hold the result.</param>
        /// <returns><see langword="true"/> if there was an outgoing packet with the label; otherwise, <see langword="false"/>.</returns>
        bool TryGetOutgoingCode(string label, out ushort code);
    }
}
