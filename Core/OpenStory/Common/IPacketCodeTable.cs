namespace OpenStory.Common
{
    /// <summary>
    /// Provides methods for looking up packet labels and op codes.
    /// </summary>
    public interface IPacketCodeTable
    {
        /// <summary>
        /// Attempts to get the label for a specified incoming packet code.
        /// </summary>
        /// <param name="code">The packet code to look up the label of.</param>
        /// <param name="label">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if there was an incoming packet code for the label; otherwise, <see langword="false"/>.</returns>
        bool TryGetIncomingLabel(ushort code, out string label);

        /// <summary>
        /// Attempts to get the outgoing packet code for a specified label.
        /// </summary>
        /// <param name="label">The label for the outgoing packet.</param>
        /// <param name="code">The variable to hold the result.</param>
        /// <returns><see langword="true"/> if there was an outgoing packet with the label; otherwise, <see langword="false"/>.</returns>
        bool TryGetOutgoingCode(string label, out ushort code);
    }
}
