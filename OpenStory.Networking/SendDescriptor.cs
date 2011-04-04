using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Common;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an asynchronous network send buffer.
    /// </summary>
    sealed class SendDescriptor : Descriptor
    {
        private AtomicBoolean isSending;
        private ConcurrentQueue<byte[]> queue;
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
            base.SocketArgs.Completed += this.EndSendAsynchronous;

            this.isSending = new AtomicBoolean(false);
            this.queue = new ConcurrentQueue<byte[]>();
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <exception cref="InvalidOperationException">Thrown if this session is not open.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is <c>null</c>.</exception>
        public void Write(byte[] data)
        {
            if (!base.Container.IsActive) throw new InvalidOperationException("The network session is not open.");
            if (data == null) throw new ArgumentNullException("data");

            this.queue.Enqueue(data);

            // For the confused: isSending.CompareExchange 
            // will return true if we're currently sending
            if (this.isSending.CompareExchange(comparand: false, newValue: true))
            {
                return;
            }

            this.sentBytes = 0;
            this.BeginSend();
        }

        #region Async send methods

        private void BeginSend()
        {
            ResetBuffer();

            try
            {
                // For the confused: Socket.SendAsync() returns false
                // if the operation completed synchronously.
                // As long as the socket operation completes synchronously,
                // this loop will handle the transfers synchronously too.
                while (!base.Container.Socket.SendAsync(base.SocketArgs))
                {
                    if (!this.EndSendSynchronous(base.SocketArgs)) break;
                }
            }
            catch (ObjectDisposedException)
            {
                this.Container.Close();
            }
        }

        /// <summary>
        /// Looks at the current segment and adjusts the SocketArgs buffer to it.
        /// </summary>
        private void ResetBuffer()
        {
            byte[] segment;
            this.queue.TryPeek(out segment);

            base.SocketArgs.SetBuffer(segment, this.sentBytes,
                                      segment.Length - this.sentBytes);
        }

        /// <summary>Synchronous EndSend operation.</summary>
        /// <remarks>
        /// This method will return true if there is more data to send.
        /// If there is no more data or if there was a connection error, it will return false.
        /// </remarks>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><c>true</c> if there is more to send; otherwise, <c>false</c>.</returns>
        private bool EndSendSynchronous(SocketAsyncEventArgs args)
        {
            if (!this.HandleTransferredData(args)) return false;

            this.ResetBuffer();

            return true;
        }

        /// <summary>
        /// Asynchronous EndSend, also the callback for the Completed event.
        /// </summary>
        /// <remarks>
        /// If there is more data to send, this method will call <see cref="BeginSend()"/>.
        /// </remarks>
        /// <param name="sender">The sender of the Completed event.</param>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        private void EndSendAsynchronous(object sender, SocketAsyncEventArgs args)
        {
            if (!this.HandleTransferredData(args))
            {
                return;
            }

            this.BeginSend();
        }

        /// <summary>
        /// Handles the data which was transferred using the given SocketAsyncEventArgs object.
        /// </summary>
        /// <remarks><para>
        /// This method advanced the <see cref="sentBytes"/> field forward 
        /// and moves to the next segment of the queue if the current 
        /// has finished sending.
        /// </para><para>
        /// If there was a connection error, this method will return false.
        /// If all the queued data has been sent, this method will set <see cref="isSending"/>
        /// to <c>false</c> and return false. Otherwise it will return true.
        /// </para></remarks>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><c>true</c> if there is more to send; otherwise, <c>false</c>.</returns>
        private bool HandleTransferredData(SocketAsyncEventArgs args)
        {
            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                base.HandleError(args);
                return false;
            }

            this.sentBytes += transferred;
            byte[] segment;
            if (this.queue.TryPeek(out segment) && segment.Length == this.sentBytes)
            {
                this.queue.TryDequeue(out segment);
                this.sentBytes = 0;
            }

            if (!this.queue.IsEmpty)
            {
                return true;
            }

            this.isSending.Exchange(newValue: false);
            return false;
        }

        #endregion

        protected override void CloseImpl()
        {
            this.queue = null;
        }
    }
}