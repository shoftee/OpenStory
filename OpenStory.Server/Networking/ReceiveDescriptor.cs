using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Cryptography;

namespace OpenStory.Server.Networking
{
    sealed class ReceiveDescriptor
    {
        private const int BufferSize = 1460;

        /// <summary>The event used to handle connection errors.</summary>
        public event EventHandler<SocketErrorEventArgs> OnError;

        private readonly IReceiveDescriptorContainer container;
        private readonly AesEncryption receiveCrypto;
        private readonly SocketAsyncEventArgs socketArgs;

        private byte[] receiveBuffer;

        private BoundedBuffer packetBuffer;

        /// <summary>Initializes a new instance of ReceiveDescriptor.</summary>
        /// <param name="container">The <see cref="IReceiveDescriptorContainer"/> containing this instance.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="container"/> is null.</exception>
        public ReceiveDescriptor(IReceiveDescriptorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.container = container;
            this.receiveCrypto = container.ReceiveCrypto;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += this.EndReceive;

            this.packetBuffer = new BoundedBuffer();

            this.ClearBuffer();
        }

        #region Buffer methods

        private void SetFreshBuffer()
        {
            this.receiveBuffer = new byte[BufferSize];
            this.ResetPositions();
        }

        private void ClearBuffer()
        {
            this.receiveBuffer = null;
            this.socketArgs.SetBuffer(null, 0, 0);
            this.packetBuffer.Reset(0);
        }

        private void ResetPositions()
        {
            this.socketArgs.SetBuffer(this.receiveBuffer, 0, BufferSize);
        }

        #endregion

        #region Async receive methods

        public void StartReceive()
        {
            this.SetFreshBuffer();

            this.BeginReceive();
        }

        private void BeginReceive()
        {
            if (this.container.IsDisconnected) return;

            try
            {
                if (!this.container.Socket.ReceiveAsync(this.socketArgs))
                {
                    this.EndReceive(null, this.socketArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                this.container.Close();
            }
        }

        private void EndReceive(object sender, SocketAsyncEventArgs args)
        {
            if (this.container.IsDisconnected) return;

            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                HandleError(args);
                return;
            }

            int remaining = this.ProcessReceivedSegment(transferred);
            if (remaining > 0)
            {
                Buffer.BlockCopy(this.receiveBuffer, transferred - remaining, this.receiveBuffer, 0, remaining);
            }
            this.socketArgs.SetBuffer(remaining, BufferSize - remaining);

            this.BeginReceive();
        }

        private void HandleError(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success &&
                args.SocketError != SocketError.ConnectionReset &&
                this.OnError != null)
            {
                this.OnError(this, new SocketErrorEventArgs(args.SocketError));
            }
            this.container.Close();
        }

        #endregion

        private int ProcessReceivedSegment(int transferred)
        {
            int position = 0, remaining = transferred;

            if (packetBuffer.FreeSpace > 0)
            {
                int bufferred = packetBuffer.AppendFill(receiveBuffer, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }

            // For the confused: if FreeSpace is not 0 at this point,
            // AppendFill() couldn't fill the buffer so we don't have any more data.
            if (this.packetBuffer.FreeSpace != 0) return 0;

            byte[] rawData = this.packetBuffer.Extract();
            this.DecryptAndHandle(rawData);

            if (remaining < 4)
            {
                // If there are less than 4 elements ahead, the header validation will blow up.
                // Tell the caller we have stuff left for next time.
                return remaining;
            }
            if (!this.receiveCrypto.CheckSegmentHeader(this.receiveBuffer, position))
            {
                this.container.Close();
                return 0;
            }
            int packetLength = AesEncryption.GetSegmentPacketLength(this.receiveBuffer, position);
            this.packetBuffer.Reset(packetLength);
            return 0;
        }

        private void DecryptAndHandle(byte[] rawData)
        {
            CustomEncryption.Decrypt(rawData);
            this.receiveCrypto.Transform(rawData);
            // TODO: send packet on its way...
        }

        public void Close()
        {
            this.ClearBuffer();
        }
    }
}