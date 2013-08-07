using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Cryptography;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for operating with a server session.
    /// </summary>
    public interface IServerSession
    {
        /// <summary>
        /// Occurs when a packet requires processing.
        /// </summary>
        event EventHandler<PacketProcessingEventArgs> PacketProcessing;

        /// <summary>
        /// Occurs when the session has pending packets waiting to be processed.
        /// </summary>
        event EventHandler ReadyForPush;

        /// <summary>
        /// Occurs right before the session is closed.
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
        /// <param name="crypto">The <see cref="EndpointCrypto"/> for the session.</param>
        /// <param name="info">The information for the handshake process.</param>
        void Start(EndpointCrypto crypto, HandshakeInfo info);

        /// <summary>
        /// Attaches a <see cref="Socket"/> to this session.
        /// </summary>
        /// <param name="socket">The <see cref="Socket"/> to attach. </param>
        void AttachSocket(Socket socket);

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <see langword="null"/>.
        /// </exception>
        void WritePacket(byte[] packet);

        /// <summary>
        /// Closes the session.
        /// </summary>
        void Close();

        /// <summary>
        /// Starts the asynchronous packet sending process.
        /// </summary>
        void Push();
    }
}