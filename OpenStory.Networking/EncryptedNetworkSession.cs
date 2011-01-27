using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Cryptography;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an encrypted network session.
    /// </summary>
    public sealed class EncryptedNetworkSession : NetworkSession
    {
        /// <summary>
        /// The event used to handle incoming packets.
        /// </summary>
        public event EventHandler<IncomingPacketEventArgs> OnIncomingPacket;

        private BoundedBuffer packetBuffer;
        private BoundedBuffer headerBuffer;

        /// <summary>
        /// Gets the <see cref="Packer"/> used to transform outgoing data.
        /// </summary>
        public Packer Packer { get; private set; }

        /// <summary>
        /// Gets the <see cref="Unpacker"/> used to transform incoming data.
        /// </summary>
        public Unpacker Unpacker { get; private set; }

        /// <summary>
        /// Initializes a new instance of the EncryptedNetworkSession class.
        /// </summary>
        /// <param name="clientSocket">The underlying socket for this network session.</param>
        /// <param name="packer">A <see cref="Packer"/> to use for outgoing data.</param>
        /// <param name="unpacker">An <see cref="Unpacker"/> to use for incoming data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the parameters is <c>null</c>.
        /// </exception>
        public EncryptedNetworkSession(Socket clientSocket, Packer packer, Unpacker unpacker)
            : base(clientSocket)
        {
            if (clientSocket == null) throw new ArgumentNullException("clientSocket");
            if (packer == null) throw new ArgumentNullException("packer");
            if (unpacker == null) throw new ArgumentNullException("unpacker");

            this.Packer = packer;
            this.Unpacker = unpacker;

            this.packetBuffer = new BoundedBuffer();
            this.headerBuffer = new BoundedBuffer(4);
        }

        /// <summary>
        /// Checks if the OnIncomingPacket event has subscribers.
        /// </summary>
        protected override void StartImpl()
        {
            if (this.OnIncomingPacket == null)
            {
                throw new InvalidOperationException("'OnIncomingPacket' has no subscribers.");
            }
        }

        #region Outgoing logic

        /// <summary>
        /// Bypasses encryption and sends a raw packet.
        /// </summary>
        /// <param name="raw">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="raw" /> is <c>null</c>.
        /// </exception>
        public void WriteRaw(byte[] raw)
        {
            if (raw == null) throw new ArgumentNullException("raw");
            base.Write(raw);
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
            if (packet == null) throw new ArgumentNullException("packet");

            byte[] rawData = this.Packer.EncryptAndPack(packet);

            base.Write(rawData);
        }

        #endregion

        #region Incoming logic

        /// <summary>
        /// Implementation of the hook for handling incoming data.
        /// </summary>
        /// <param name="data">The incoming data.</param>
        protected override void HandleIncomingData(byte[] data)
        {
            int position = 0, remaining = data.Length;
            while (packetBuffer.FreeSpace == 0)
            {
                int headerRemaining = this.headerBuffer.FreeSpace;
                int bufferred;
                if (headerRemaining > 0)
                {
                    bufferred = this.headerBuffer.AppendFill(data, headerRemaining);

                    // For the confused: if we didn't fill the header, it 
                    // means the data array had less elements than we need.
                    // We move on.
                    if (bufferred < headerRemaining) break;

                    position += bufferred;
                    remaining -= bufferred;
                }

                byte[] header = this.headerBuffer.ExtractAndReset(4);
                if (this.Unpacker.CheckHeader(header))
                {
                    this.Close();
                }

                int length = AesTransform.GetPacketLength(header);
                byte[] rawData = packetBuffer.ExtractAndReset(length);
                if (rawData.Length > 0)
                {
                    this.DecryptAndHandle(rawData);
                }

                bufferred = packetBuffer.AppendFill(data, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }
        }

        private void DecryptAndHandle(byte[] data)
        {
            this.Unpacker.Decrypt(data);

            var args = new IncomingPacketEventArgs(data);
            this.OnIncomingPacket(this, args);
        }

        #endregion

        /// <summary>
        /// Removes the subscribers to the OnIncomingPacket event.
        /// </summary>
        protected override void CloseImpl()
        {
            this.OnIncomingPacket = null;
        }
    }
}
