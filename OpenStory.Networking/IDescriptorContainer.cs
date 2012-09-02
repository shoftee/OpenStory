using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// A container for <see cref="Descriptor"/> instances.
    /// </summary>
    internal interface IDescriptorContainer
    {
        /// <summary>
        /// Gets the network socket for the session.
        /// </summary>
        Socket Socket { get; }

        /// <summary>
        /// Gets whether the session is active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Closes the session.
        /// </summary>
        void Close();
    }
}
