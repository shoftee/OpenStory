using OpenStory.Common.IO;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for creating packets.
    /// </summary>
    public interface IPacketFactory
    {
        /// <summary>
        /// Creates a new <see cref="IPacketBuilder"/> object for the specified packet label.
        /// </summary>
        /// <param name="label">The label of the packet type.</param>
        /// <returns>a new <see cref="IPacketBuilder"/> initialized with the packet code for the specified label.</returns>
        PacketBuilder CreatePacket(string label);
    }
}
