using System;
using OpenStory.Common.IO;
using OpenStory.Cryptography;
using OpenStory.Server.Processing;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides methods for operating with a server session.
    /// </summary>
    public interface IServerSession
    {
        /// <summary>
        /// The event is raised when a packet requires processing.
        /// </summary>
        event EventHandler<PacketProcessingEventArgs> PacketProcessing;

        /// <summary>
        /// The event is raised right before the session is closed.
        /// </summary>
        event EventHandler Closing;

        /// <summary>
        /// A unique 32-bit network session identifier.
        /// </summary>
        /// <remarks>
        /// This session identifier and the account session identifier are different things.
        /// </remarks>
        int NetworkSessionId { get; }

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        /// <param name="factory">The <see cref="RollingIvFactory"/> to use to create <see cref="RollingIv"/> instances.</param>
        /// <param name="info">The information for the handshake process.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="factory"/> or <paramref name="info"/> are <c>null</c>.
        /// </exception>
        void Start(RollingIvFactory factory, HandshakeInfo info);

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        void WritePacket(byte[] packet);

        /// <summary>
        /// Closes the session.
        /// </summary>
        void Close();
    }
}