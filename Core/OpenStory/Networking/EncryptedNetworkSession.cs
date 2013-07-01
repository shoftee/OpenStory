using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
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
    public abstract class EncryptedNetworkSession : IDisposable
    {
        #region Events

        /// <summary>
        /// The event is raised when an incoming packet is fully received.
        /// </summary>
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;

        /// <summary>
        /// The event is raised when the session begins closing.
        /// </summary>
        public event EventHandler Closing;

        #endregion

        private bool isDisposed;

        #region Properties

        /// <summary>
        /// Gets the buffer used to store packet headers.
        /// </summary>
        protected BoundedBuffer HeaderBuffer { get; private set; }

        /// <summary>
        /// Gets the buffer used to store packet data.
        /// </summary>
        protected BoundedBuffer PacketBuffer { get; private set; }

        /// <summary>
        /// Gets or sets the internal NetworkSession instance.
        /// </summary>
        protected NetworkSession Session { get; private set; }

        /// <summary>
        /// Gets or sets the cryptographic transformer for this session.
        /// </summary>
        protected EndpointCrypto Crypto { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedNetworkSession"/> class with no specified socket.
        /// </summary>
        /// <remarks>
        /// Initializes the internal fields and <see cref="Session"/> with no specified socket.
        /// Call <see cref="AttachSocket(Socket)"/> before starting the network operations.
        /// </remarks>
        protected EncryptedNetworkSession()
        {
            this.PacketBuffer = new BoundedBuffer();
            this.HeaderBuffer = new BoundedBuffer(4);

            this.Session = new NetworkSession();
            this.Session.DataArrived += this.OnDataArrived;
            this.Session.Closing += this.OnClosing;

            this.isDisposed = false;
        }

        /// <summary>
        /// Attaches a <see cref="Socket"/> to this ServerSession instance.
        /// </summary>
        /// <param name="socket">The Socket to attach. </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket"/> is <c>null</c>.
        /// </exception>
        public void AttachSocket(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            this.Session.AttachSocket(socket);
        }

        private void OnClosing(object sender, EventArgs e)
        {
            var handler = this.Closing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close()
        {
            this.PacketReceived = null;

            this.Session.Close();
            this.Closing = null;
        }

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        public void WritePacket(byte[] packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (this.Session.Socket.Connected)
            {
                byte[] rawData = this.Crypto.EncryptAndPack(packet.FastClone());
                this.Session.Write(rawData);
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
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="args"/> is <c>null</c>.</exception>
        protected virtual void OnDataArrived(object sender, DataArrivedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

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
                    this.Close();
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
            this.PacketReceived(this, args);
        }

        /// <summary>
        /// Checks if the <see cref="PacketReceived"/> event has a subscriber and throws 
        /// <see cref="InvalidOperationException"/> if not.
        /// </summary>
        protected void ThrowIfNoPacketReceivedSubscriber()
        {
            if (this.PacketReceived == null)
            {
                throw new InvalidOperationException(Exceptions.ReceiveEventHasNoSubscribers);
            }
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
                var session = this.Session;
                if (session != null)
                {
                    session.Dispose();
                }

                var header = this.HeaderBuffer;
                if (header != null)
                {
                    header.Dispose();
                }

                var packet = this.PacketBuffer;
                if (packet != null)
                {
                    packet.Dispose();
                }

                this.Session = null;
                this.HeaderBuffer = null;
                this.PacketBuffer = null;

                this.isDisposed = true;
            }
        }

        #endregion
    }
}
