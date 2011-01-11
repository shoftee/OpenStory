using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenMaple.Threading;

namespace OpenMaple.Networking
{
    class SendDescriptor
    {
        public event EventHandler<SocketErrorEventArgs> OnError;

        private IDescriptorContainer container;

        private AtomicBoolean isSending;
        private SocketAsyncEventArgs socketArgs;
        private ConcurrentQueue<ArraySegment<byte>> queue;
        private int sentBytes;

        public SendDescriptor(IDescriptorContainer container)
        {
            this.container = container;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += this.EndSend;

            this.Open();
        }

        /// <summary>
        /// Writes data to the network stream.
        /// </summary>
        /// <param name="data">The data to send.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="data"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The exception is thrown when this method is called on an inactive connection.</exception>
        public void Send(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (this.container.IsDisconnected)
            {
                throw new InvalidOperationException("Buffer not set, call SetBuffer for this descriptor before you use it.");
            }
            var segment = new ArraySegment<byte>(data);
            this.queue.Enqueue(segment);

            // For the confused: isSending.CompareExchange 
            // will return true if we're currently sending
            if (!this.isSending.CompareExchange(false, true))
            {
                this.sentBytes = 0;
                this.BeginSend();
            }
        }

        private void BeginSend()
        {
            ArraySegment<byte> segment;
            if (!this.queue.TryPeek(out segment))
            {
                throw new InvalidOperationException("The send queue is empty.");
            }

            this.socketArgs.SetBuffer(segment.Array,
                segment.Offset + this.sentBytes,
                segment.Count - this.sentBytes);
            try
            {
                // For the confused: Socket.SendAsync() returns false
                // if the operation completed synchronously.
                if (!this.container.Socket.SendAsync(socketArgs))
                {
                    this.EndSend(null, socketArgs);
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
                if (args.SocketError != SocketError.Success && OnError != null)
                {
                    OnError(this, new SocketErrorEventArgs(args.SocketError));
                }
                this.container.Close();
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
                this.isSending.Exchange(false);
            }
        }

        public void Close()
        {
            this.queue = null;
        }

        public void Open()
        {
            this.isSending = new AtomicBoolean(false);
            this.queue = new ConcurrentQueue<ArraySegment<byte>>();
        }
    }
}