using System;
using System.ComponentModel;
using System.Net.Sockets;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a base class for encrypted network sessions.
    /// </summary>
    /// <remarks>
    /// This class provides the packet buffering and decryption logic for inbound packets,
    /// as well as the logic to write outbound packets.
    /// </remarks>
    [Localizable(true)]
    public abstract class EncryptedNetworkSession : INetworkSession, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when an incoming packet is fully received.
        /// </summary>
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;

        /// <summary>
        /// Occurs when the session begins closing.
        /// </summary>
        public event EventHandler<ConnectionClosingEventArgs> Closing;

        /// <summary>
        /// Occurs when there's a connection error.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> SocketError;

        #endregion

        private bool isDisposed;
        private NetworkSession baseSession;
        private BoundedBuffer headerBuffer;
        private BoundedBuffer packetBuffer;

        #region Properties

        /// <summary>
        /// Gets the buffer used to store packet headers.
        /// </summary>
        protected BoundedBuffer HeaderBuffer
        {
            get { return this.headerBuffer; }
        }

        /// <summary>
        /// Gets the buffer used to store packet data.
        /// </summary>
        protected BoundedBuffer PacketBuffer
        {
            get { return this.packetBuffer; }
        }

        /// <summary>
        /// Gets the underlying network session.
        /// </summary>
        protected NetworkSession BaseSession
        {
            get { return this.baseSession; }
        }

        /// <summary>
        /// Gets or sets the cryptographic transformer for this session.
        /// </summary>
        protected EndpointCrypto Crypto { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedNetworkSession"/> class with no specified socket.
        /// </summary>
        /// <remarks>
        /// Call <see cref="AttachSocket(Socket)"/> before starting the network operations.
        /// </remarks>
        protected EncryptedNetworkSession()
        {
            this.packetBuffer = new BoundedBuffer();
            this.headerBuffer = new BoundedBuffer(4);

            this.baseSession = this.CreateInnerSession();

            this.isDisposed = false;
        }

        private NetworkSession CreateInnerSession()
        {
            var s = new NetworkSession();
            s.DataArrived += this.OnDataArrived;
            s.Closing += this.OnClosing;
            s.SocketError += this.OnSocketError;
            return s;
        }

        private void OnSocketError(object sender, SocketErrorEventArgs e)
        {
            this.SocketError?.Invoke(this, e);
        }

        private void OnClosing(object sender, ConnectionClosingEventArgs e)
        {
            this.Closing?.Invoke(this, e);
        }

        /// <summary>
        /// Attaches a <see cref="Socket"/> to this session.
        /// </summary>
        /// <param name="sessionSocket">The <see cref="Socket"/> to attach. </param>
        public void AttachSocket(Socket sessionSocket)
        {
            this.baseSession.AttachSocket(sessionSocket);
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close(string reason)
        {
            this.PacketReceived = null;

            this.baseSession.Close(reason);

            this.SocketError = null;
            this.Closing = null;
        }

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <see langword="null"/>.
        /// </exception>
        public void WritePacket(byte[] packet)
        {
            Guard.NotNull(() => packet, packet);

            if (this.baseSession.IsActive)
            {
                byte[] rawData = this.Crypto.EncryptAndPack(packet.FastClone());
                this.baseSession.Write(rawData);
            }
        }

        /// <summary>
        /// Called when the <see cref="PacketReceived"/> event is raised.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class, call the base implementation after your logic.
        /// </remarks>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The packet data that was received.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="args"/> is <see langword="null"/>.</exception>
        protected virtual void OnDataArrived(object sender, DataArrivedEventArgs args)
        {
            Guard.NotNull(() => args, args);

            byte[] data = args.Data;
            int position = 0, remaining = data.Length;
            while (this.PacketBuffer.FreeSpace == 0)
            {
                byte[] rawData = this.PacketBuffer.ExtractAndReset(0);
                if (rawData.Length > 0)
                {
                    this.Crypto.Decrypt(rawData);

                    var incomingPacketArgs = new PacketReceivedEventArgs(rawData);
                    this.OnPacketReceived(incomingPacketArgs);
                }

                if (remaining == 0)
                {
                    break;
                }

                int bufferred;
                int headerRemaining = this.HeaderBuffer.FreeSpace;
                if (headerRemaining > 0)
                {
                    bufferred = this.HeaderBuffer.AppendFill(data, position, headerRemaining);

                    // For the confused: if we didn't fill the header, it 
                    // means the data array didn't have enough elements.
                    // We move on.
                    if (bufferred < headerRemaining)
                    {
                        break;
                    }

                    position += bufferred;
                    remaining -= bufferred;
                }

                byte[] header = this.HeaderBuffer.ExtractAndReset(4);
                int length;
                if (!this.Crypto.TryGetLength(header, out length))
                {
                    this.Close(@"Could not decode packet length.");
                    return;
                }

                this.PacketBuffer.Reset(length);

                bufferred = this.PacketBuffer.AppendFill(data, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }
        }

        private void OnPacketReceived(PacketReceivedEventArgs args)
        {
            this.PacketReceived.Invoke(this, args);
        }

        /// <summary>
        /// Checks if the <see cref="PacketReceived"/> event has a subscriber and throws 
        /// <see cref="InvalidOperationException"/> if not.
        /// </summary>
        protected void ThrowIfNoPacketReceivedSubscriber()
        {
            if (this.PacketReceived == null)
            {
                throw new InvalidOperationException(CommonStrings.ReceiveEventHasNoSubscribers);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var networkSession = this.baseSession;
            return networkSession == null? @"No session" : networkSession.ToString();
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Runs the disposal operations for the base class.
        /// </summary>
        /// <param name="disposing">Whether the method is being called for disposal or finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.isDisposed)
            {
                Misc.AssignNullAndDispose(ref this.baseSession);
                Misc.AssignNullAndDispose(ref this.headerBuffer);
                Misc.AssignNullAndDispose(ref this.packetBuffer);

                this.isDisposed = true;
            }
        }

        #endregion
    }
}
