using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for operating with a server session.
    /// </summary>
    public interface IServerSession : INetworkSession
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
        /// A unique 32-bit network session identifier.
        /// </summary>
        /// <remarks>
        /// This session identifier and the account session identifier are different things.
        /// </remarks>
        int NetworkSessionId { get; }

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        /// <param name="endpointCrypto">The <see cref="EndpointCrypto"/> for the session.</param>
        /// <param name="handshakeInfo">The information for the handshake process.</param>
        void Start(EndpointCrypto endpointCrypto, HandshakeInfo handshakeInfo);

        /// <summary>
        /// Attaches a <see cref="Socket"/> to this session.
        /// </summary>
        /// <param name="sessionSocket">The <see cref="Socket"/> to attach. </param>
        void AttachSocket(Socket sessionSocket);

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
        /// Starts the asynchronous packet sending process.
        /// </summary>
        void Push();
    }
}