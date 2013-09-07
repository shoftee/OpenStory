using System;
using System.Collections.Concurrent;
using Ninject.Extensions.Logging;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Networking;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents an encrypted network session.
    /// </summary>
    internal sealed class ServerSession : EncryptedNetworkSession, IServerSession
    {
        private static readonly AtomicInteger RollingNetworkSessionId = new AtomicInteger(0);

        #region Fields and properties

        /// <inheritdoc />
        public event EventHandler ReadyForPush;

        /// <inheritdoc />
        public event EventHandler<PacketProcessingEventArgs> PacketProcessing;

        private readonly ConcurrentQueue<byte[]> packets;
        private readonly AtomicBoolean isPushing;
        private readonly IPacketCodeTable packetCodeTable;
        private readonly ILogger logger;

        /// <inheritdoc />
        /// <remarks>
        /// This session identifier and the account session identifier are different things.
        /// </remarks>
        public int NetworkSessionId { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSession"/> class.
        /// </summary>
        public ServerSession(
            IPacketCodeTable packetCodeTable, 
            ILogger logger)
        {
            this.NetworkSessionId = RollingNetworkSessionId.Increment();

            this.isPushing = false;
            this.packets = new ConcurrentQueue<byte[]>();

            this.packetCodeTable = packetCodeTable;
            this.logger = logger;

            this.PacketReceived += this.HandlePacketReceived;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="crypto"/> or <paramref name="info"/> are <see langword="null"/>.
        /// </exception>
        public void Start(EndpointCrypto crypto, HandshakeInfo info)
        {
            if (crypto == null)
            {
                throw new ArgumentNullException("crypto");
            }

            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            this.ThrowIfNoPacketReceivedSubscriber();

            this.Crypto = crypto;
            byte[] handshake = ConstructHandshakePacket(info);

            logger.Debug(@"Network session {0} started.", this.NetworkSessionId);
            this.Session.Start();
            this.Session.Write(handshake);
        }

        #region Outgoing logic

        private static byte[] ConstructHandshakePacket(HandshakeInfo info)
        {
            using (var builder = new PacketBuilder())
            {
                builder.WriteInt16(info.Header);
                builder.WriteInt16(info.Version);
                builder.WriteLengthString(info.Subversion);
                builder.WriteBytes(info.ClientIv);
                builder.WriteBytes(info.ServerIv);

                // Locale ID (used for localizations and test servers)
                builder.WriteByte(info.LocaleId);

                return builder.ToByteArray();
            }
        }

        #endregion

        #region Pushing events

        /// <summary>
        /// Starts the asynchronous packet sending process.
        /// </summary>
        public void Push()
        {
            if (this.PacketProcessing == null)
            {
                throw new InvalidOperationException(ServerStrings.PacketProcessingEventHasNoSubscriber);
            }

            // CompareExchange returns the original value, hence:
            // => true means we were already pushing, don't start a second one.
            // => false means we were not pushing and we just toggled it, so we should start now.
            if (!this.isPushing.CompareExchange(comparand: false, newValue: true))
            {
                this.StartPushing();
            }
        }

        private void StartPushing()
        {
            byte[] packet;
            bool success = this.packets.TryDequeue(out packet);

            // This can only be false when we are called by the asynchronous continuation,
            // which doesn't have a good way of communicating its results to us,
            // so it can't dequeue the next packet and pass it. So we do it here instead <3
            if (success)
            {
                // PushAsync will return false if we completed synchronously
                // and we would therefore like to try pushing another one right away.
                while (!this.PushAsync(packet))
                {
                    // ContinuePushSynchronous will return false if there are no more packets to push.
                    if (!this.ContinuePushSynchronous(out packet))
                    {
                        break;
                    }
                }
            }
            else
            {
                this.isPushing.Exchange(newValue: false);
            }
        }

        private bool PushAsync(byte[] packet)
        {
            var reader = new PacketReader(packet);

            ushort packetCode;
            if (!reader.TryReadUInt16(out packetCode))
            {
                this.logger.Debug(@"A packet without a packet code was received.");
                this.Close();

                // Bad packet. We kill the session and stop pushing.
                return true;
            }

            string label;
            if (!this.packetCodeTable.TryGetIncomingLabel(packetCode, out label))
            {
                this.logger.Debug(@"Unknown Packet Code 0x{0:4X} - {1}", packetCode, reader.ReadFully().ToHex());

                // Bad server. Take the blame and try the next one! :<
                return false;
            }

            // Invoke event asynchronously.
            var args = new PacketProcessingEventArgs(label, reader);
            var result = this.BeginInvoke(args);

            // If we completed synchronously, we'll take another one right away.
            // Otherwise, leave it to the asynchronous continuation.
            return result.CompletedSynchronously;
        }

        private IAsyncResult BeginInvoke(PacketProcessingEventArgs args)
        {
            var handler = this.PacketProcessing;
            var result = handler.BeginInvoke(this, args, this.ContinuePushAsynchronous, null);
            return result;
        }

        private bool ContinuePushSynchronous(out byte[] packet)
        {
            bool hasNext = this.packets.TryDequeue(out packet);

            if (!hasNext)
            {
                this.isPushing.Exchange(newValue: false);
            }

            return hasNext;
        }

        private void ContinuePushAsynchronous(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                this.StartPushing();
            }
        }

        #endregion

        #region Packet arrival handling

        private void HandlePacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var bytes = e.Reader.ReadFully();

            this.packets.Enqueue(bytes);

            if (!this.isPushing.Value)
            {
                this.OnReadyForPush();
            }
        }

        private void OnReadyForPush()
        {
            var handler = this.ReadyForPush;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Logging

        #endregion
    }
}
