using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Common;

namespace OpenStory.Networking
{
    /// <summary>
    /// A send buffer for a <see cref="NetworkSession"/>.
    /// </summary>
    sealed class SendDescriptor : Descriptor
    {
        private AtomicBoolean isSending;
        private ConcurrentQueue<ArraySegment<byte>> queue;
        private int sentBytes;

        /// <summary>
        /// Initializes a new instance of the SendDescriptor class.
        /// </summary>
        /// <param name="container">The <see cref="IDescriptorContainer"/> containing this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container" /> is <c>null</c>.
        /// </exception>
        public SendDescriptor(IDescriptorContainer container)
            : base(container)
        {
            base.SocketArgs.Completed += this.EndSend;

            this.isSending = new AtomicBoolean(false);
            this.queue = new ConcurrentQueue<ArraySegment<byte>>();
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <exception cref="InvalidOperationException">Thrown if this session is not open.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <c>null</c>.</exception>
        public void Write(byte[] data)
        {
            if (base.Container.IsDisconnected) throw new InvalidOperationException("The network session is not open.");
            if (data == null) throw new ArgumentNullException("data");

            this.Send(data);
        }

        #region Async send methods

        private void Send(byte[] data)
        {
            var segment = new ArraySegment<byte>(data);
            this.queue.Enqueue(segment);

            // For the confused: isSending.CompareExchange 
            // will return true if we're currently sending
            if (this.isSending.CompareExchange(comparand: false, newValue: true))
            {
                return;
            }

            this.sentBytes = 0;
            this.BeginSend();
        }

        private void BeginSend()
        {
            ArraySegment<byte> segment;
            this.queue.TryPeek(out segment);

            base.SocketArgs.SetBuffer(segment.Array,
                                      segment.Offset + this.sentBytes,
                                      segment.Count - this.sentBytes);
            try
            {
                // For the confused: Socket.SendAsync() returns false
                // if the operation completed synchronously.
                if (!base.Container.Socket.SendAsync(base.SocketArgs))
                {
                    this.EndSend(null, base.SocketArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                this.Container.Close();
            }
        }

        private void EndSend(object sender, SocketAsyncEventArgs args)
        {
            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                base.RaiseErrorEvent(args);
                return;
            }

            this.sentBytes += transferred;
            ArraySegment<byte> segment;
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

        #endregion

        protected override void CloseImpl()
        {
            this.queue = null;
        }
    }
}