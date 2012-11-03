using System.Collections.Generic;
using OpenStory.Common.IO;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for processing packets.
    /// </summary>
    /// <typeparam name="TClient">The type of the client.</typeparam>
    public interface IPacketProcessor<TClient>
        where TClient : ClientBase
    {
        /// <summary>
        /// Retrieves a list of packet labels that the processor can handle.
        /// </summary>
        /// <returns>a list of labels that the processor can handle.</returns>
        IEnumerable<string> GetKnownLabels();

        /// <summary>
        /// Checks whether the provided packet is for a valid operation relative to the current state of the client.
        /// </summary>
        /// <param name="label">The label for the packet.</param>
        /// <param name="client">The client instance.</param>
        /// <returns><c>true</c> if the operation is valid; otherwise, <c>false</c>.</returns>
        bool ValidateState(string label, TClient client);

        /// <summary>
        /// Processes the provided packet
        /// </summary>
        /// <param name="client">The client instance.</param>
        /// <param name="label">The label for the packet.</param>
        /// <param name="reader">The packet reader instance.</param>
        void ProcessPacket(TClient client, string label, IUnsafePacketReader reader);
    }
}
