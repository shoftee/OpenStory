using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Common.Threading;
using OpenStory.Cryptography;
using OpenStory.Server.Synchronization;

namespace OpenStory.Server.Networking
{
    class SendDescriptor
    {
        public event EventHandler<SocketErrorEventArgs> OnError;

        private readonly ISendDescriptorContainer container;
        private readonly AesEncryption sendCrypto;

        private AtomicBoolean isSending;
        private ConcurrentQueue<ArraySegment<byte>> queue;
        private int sentBytes;
        private SocketAsyncEventArgs socketArgs;

        public SendDescriptor(ISendDescriptorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            this.container = container;
            this.sendCrypto = container.SendCrypto;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += this.EndSend;

            this.Open();
        }

        public void Open()
        {
            this.isSending = new AtomicBoolean(false);
            this.queue = new ConcurrentQueue<ArraySegment<byte>>();
        }

        /// <summary>Encrypts a packet, adds a header to it, and writes it to the output stream.</summary>
        /// <param name="data">The packet data to write.</param>
        /// <exception cref="InvalidOperationException">Thrown if this session is not open.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <c>null</c>.</exception>
        public void Write(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            // This is the only method that modifies SendCrypto.
            Synchronizer.ScheduleAction(() => this.EncryptAndWrite(data));
        }

        public void WriteDirectly(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");

            this.Send(data);
        }

        /// <summary>Encrypts the given data as a packet and writes it to the network stream.</summary>
        /// <remarks>This method is to be used only within synchronization queues.</remarks>
        /// <param name="packet">The data to send.</param>
        private void EncryptAndWrite(byte[] packet)
        {
            int length = packet.Length;
            var rawData = new byte[length + 4];

            byte[] header = this.sendCrypto.ConstructHeader(length);
            Buffer.BlockCopy(header, 0, rawData, 0, 4);

            var encrypted = new byte[length];
            Buffer.BlockCopy(packet, 0, encrypted, 0, length);
            this.sendCrypto.Transform(encrypted);
            CustomEncryption.Encrypt(encrypted);

            Buffer.BlockCopy(encrypted, 0, rawData, 4, length);

            this.Send(rawData);
        }

        #region Async send methods

        private void Send(byte[] data)
        {
            var segment = new ArraySegment<byte>(data);
            this.queue.Enqueue(segment);

            // For the confused: isSending.CompareExchange 
            // will return true if we're currently sending
            if (this.isSending.CompareExchange(false, newValue: true)) return;

            this.sentBytes = 0;
            this.BeginSend();
        }

        private void BeginSend()
        {
            ArraySegment<byte> bufferSegment;
            if (!this.queue.TryPeek(out bufferSegment))
            {
                throw new InvalidOperationException("The send queue is empty.");
            }

            this.socketArgs.SetBuffer(bufferSegment.Array,
                                      bufferSegment.Offset + this.sentBytes,
                                      bufferSegment.Count - this.sentBytes);
            try
            {
                // For the confused: Socket.SendAsync() returns false
                // if the operation completed synchronously.
                if (!this.container.Socket.SendAsync(this.socketArgs))
                {
                    this.EndSend(null, this.socketArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                this.container.Close();
            }
        }

        private void EndSend(object sender, SocketAsyncEventArgs args)
        {
            int bytes = args.BytesTransferred;
            if (bytes <= 0)
            {
                HandleError(args);
                return;
            }

            ArraySegment<byte> segment;
            this.sentBytes += bytes;
            if (this.queue.TryPeek(out segment) && segment.Count == this.sentBytes)
            {
                this.queue.TryDequeue(out segment);
                this.sentBytes = 0;
            }

            if (!this.queue.IsEmpty)
            {
                this.BeginSend();
            }
            else
            {
                this.isSending.Exchange(newValue: false);
            }
        }

        private void HandleError(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success && this.OnError != null)
            {
                this.OnError(this, new SocketErrorEventArgs(args.SocketError));
            }
            this.container.Close();
        }

        #endregion

        public void Close()
        {
            this.queue = null;
        }
    }
}