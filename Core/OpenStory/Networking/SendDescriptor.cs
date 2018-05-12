using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using OpenStory.Common;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an asynchronous network send buffer.
    /// </summary>
    [Localizable(true)]
    internal sealed class SendDescriptor : DescriptorBase
    {
        private readonly AtomicBoolean _isSending;

        private ConcurrentQueue<byte[]> _queue;
        private int _sentBytes;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendDescriptor"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IDescriptorContainer"/> containing this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container" /> is <see langword="null"/>.
        /// </exception>
        public SendDescriptor(IDescriptorContainer container)
            : base(container)
        {
            SocketArgs.Completed += EndSendAsynchronous;

            _isSending = new AtomicBoolean(false);
            _queue = new ConcurrentQueue<byte[]>();
        }

        /// <summary>
        /// Writes a byte array to the stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if this session is not open.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="data"/> is <see langword="null"/>.
        /// </exception>
        public void Write(byte[] data)
        {
            if (!Container.IsActive)
            {
                throw new InvalidOperationException(CommonStrings.SessionIsNotActive);
            }

            Guard.NotNull(() => data, data);

            _queue.Enqueue(data);

            // If the sending operations are already in progress, 
            // they'll get to the packet we just queued eventually...
            if (_isSending.FlipIf(false))
            {
                // Otherwise, we do it ourselves.
                _sentBytes = 0;
                BeginSend();
            }
        }

        protected override void OnClosed()
        {
            _queue = new ConcurrentQueue<byte[]>();
        }

        #region Async send methods

        private void BeginSend()
        {
            // For the unknowing: There are a few tricks in motion here.
            // First of all, we use a concurrent queue to make sure we 
            // don't mess up when /adding/ packets to the queue.
            // We never actually do concurrent dequeuing of items.
            // This allows us to do a few things that we should otherwise
            // be wary of, as explained in the methods around here.
            ResetBuffer();

            try
            {
                // For the confused: Socket.SendAsync() returns false
                // if the operation completed synchronously.
                // As long as the socket operation completes synchronously,
                // this loop will handle the transfers synchronously too.
                while (!Container.Socket.SendAsync(SocketArgs))
                {
                    if (!EndSendSynchronous(SocketArgs))
                    {
                        break;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Container.Close(@"Socket disposed.");
            }
        }

        /// <summary>
        /// Looks at the current segment and adjusts the SocketArgs buffer to it.
        /// </summary>
        private void ResetBuffer()
        {
            // This method is only called when we are sure we have a packet 
            // waiting to be sent. Otherwise, segment may be null after the 
            // TryPeek call below.
            byte[] segment;
            _queue.TryPeek(out segment);

            // Since we're sure segment is not null, we can dereference it.
            SocketArgs.SetBuffer(segment, _sentBytes, segment.Length - _sentBytes);
        }

        /// <summary>
        /// Synchronous EndSend.
        /// </summary>
        /// <remarks>
        /// This method will return true if there is more data to send.
        /// If there is no more data or if there was a connection error, it will return false.
        /// </remarks>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><see langword="true"/> if there is more to send; otherwise, <see langword="false"/>.</returns>
        private bool EndSendSynchronous(SocketAsyncEventArgs args)
        {
            if (HandleTransferredData(args))
            {
                ResetBuffer();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronous EndSend, also the callback for the Completed event.
        /// </summary>
        /// <remarks>
        /// If there is more data to send, this method will call <see cref="BeginSend()"/>.
        /// </remarks>
        private void EndSendAsynchronous(object sender, SocketAsyncEventArgs args)
        {
            if (HandleTransferredData(args))
            {
                BeginSend();
            }
        }

        /// <summary>
        /// Handles the data which was transferred using the given SocketAsyncEventArgs object.
        /// </summary>
        /// <remarks><para>
        /// This method advances the <see cref="_sentBytes"/> field forward 
        /// and moves to the next segment in the queue if the current 
        /// has finished sending.
        /// </para><para>
        /// If there was a connection error, this method will return <see langword="false"/>.
        /// If all the queued data has been sent, this method will set <see cref="_isSending"/>
        /// to <see langword="false"/> and return <see langword="false"/>. Otherwise it will return <see langword="true"/>.
        /// </para></remarks>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><see langword="true"/> if there is more data to send; otherwise, <see langword="false"/>.</returns>
        private bool HandleTransferredData(SocketAsyncEventArgs args)
        {
            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                // BytesTransferred is set to -1 if the socket was closed before the 
                // last operation ended. This may happen because we requested it,
                // or because of a network failure. OnError actually checks for this.
                OnError(args);
                return false;
            }

            // We adjust the number of sent bytes.
            _sentBytes += transferred;

            // As with ResetBuffer(), this method will be called only when we have
            // a packet waiting to be sent still in the queue.
            // Since we don't do concurrent dequeuing, there is also no race condition.
            // Hence, the TryPeek and TryDequeue will not set segment to null,
            // and are guaranteed to use the same element.
            byte[] segment;
            if (_queue.TryPeek(out segment) && segment.Length == _sentBytes)
            {
                // All of the bytes in the segment were sent.
                // Thus here we can safely take it out.
                _queue.TryDequeue(out segment);
                _sentBytes = 0;
            }

            // Again, no race condition here. We can be sure that when we return true
            // and we start another send operation, the queue will have at least one 
            // waiting packet.
            if (!_queue.IsEmpty)
            {
                return true;
            }

            // Atomically set isSending to false.
            // We /do/ need to worry about this one, because it's the barrier
            // for the start of the sending process, and any thread can request 
            // a packet to be sent.
            _isSending.Set(false);
            return false;
        }

        #endregion
    }
}
