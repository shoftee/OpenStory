using System;
using System.ComponentModel;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an asynchronous network receive buffer.
    /// </summary>
    [Localizable(true)]
    internal sealed class ReceiveDescriptor : DescriptorBase
    {
        /// <summary>
        /// The default buffer size, set to match the RollingIv block size.
        /// </summary>
        private const int BufferSize = 1460;

        /// <summary>
        /// Occurs when a data segment has been successfully received.
        /// </summary>
        /// <remarks><para>
        /// This event supports only one subscriber. Attempts to subscribe more than once will throw 
        /// <see cref="InvalidOperationException"/>.
        /// </para><para>
        /// The buffered data is not persistent. After the event is raised, the underlying data will 
        /// be overwritten by a new segment. Because of this, the subscriber should either use the 
        /// data immediately or copy it into another buffer.
        /// </para></remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown when attempting to subscribe to the event when there is already one subscriber.
        /// </exception>
        public event EventHandler<DataArrivedEventArgs> DataArrived
        {
            add
            {
                if (DataArrivedInternal != null)
                {
                    throw new InvalidOperationException(CommonStrings.EventMustHaveOnlyOneSubscriber);
                }

                DataArrivedInternal += value;
            }
            remove
            {
                DataArrivedInternal -= value;
            }
        }

        private event EventHandler<DataArrivedEventArgs> DataArrivedInternal;

        private byte[] _receiveBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiveDescriptor"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IDescriptorContainer"/> for the new instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container"/> is <see langword="null"/>.
        /// </exception>
        public ReceiveDescriptor(IDescriptorContainer container)
            : base(container)
        {
            SocketArgs.Completed += EndReceiveAsynchronous;

            ClearBuffer();
        }

        #region Buffer methods

        private void SetFreshBuffer()
        {
            _receiveBuffer = new byte[BufferSize];
            ResetPositions();
        }

        private void ClearBuffer()
        {
            _receiveBuffer = null;
            SocketArgs.SetBuffer(null, 0, 0);
        }

        private void ResetPositions()
        {
            SocketArgs.SetBuffer(_receiveBuffer, 0, BufferSize);
        }

        #endregion

        #region Async receive methods

        /// <summary>
        /// Starts the receive process.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="DataArrived"/> event has no subscribers.
        /// </exception>
        public void StartReceive()
        {
            if (DataArrivedInternal == null)
            {
                throw new InvalidOperationException(CommonStrings.ReceiveEventHasNoSubscribers);
            }

            SetFreshBuffer();

            BeginReceive();
        }

        private void BeginReceive()
        {
            if (!Container.IsActive)
            {
                return;
            }

            try
            {
                // For the confused: ReceiveAsync will return false if
                // the operation completed synchronously.
                // As long as that is happening, this loop will handle
                // the data transfer synchronously as well.
                while (!Container.Socket.ReceiveAsync(SocketArgs))
                {
                    if (!EndReceiveSynchronous(SocketArgs))
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
        /// Asynchronous EndReceive, also the callback for the Completed event.
        /// </summary>
        /// <remarks>
        /// If there is more data to send, this method will call <see cref="BeginReceive()"/>.
        /// </remarks>
        private void EndReceiveAsynchronous(object sender, SocketAsyncEventArgs args)
        {
            if (EndReceiveSynchronous(args))
            {
                BeginReceive();
            }
        }

        /// <summary>
        /// Synchronous EndReceive.
        /// </summary>
        /// <param name="args">The <see cref="SocketAsyncEventArgs"/> object for this operation.</param>
        /// <returns><see langword="true"/> if there is more data to send; otherwise, <see langword="false"/>.</returns>
        private bool EndReceiveSynchronous(SocketAsyncEventArgs args)
        {
            return HandleTransferredData(args);
        }

        /// <summary>
        /// Handles the transferred data for the operation.
        /// </summary>
        /// <remarks>
        /// This method returns <see langword="false"/> on connection errors.
        /// </remarks>
        /// <param name="args">The <see cref="SocketAsyncEventArgs"/> object for this operation.</param>
        /// <returns><see langword="true"/> if there is more data to send; otherwise, <see langword="false"/>.</returns>
        private bool HandleTransferredData(SocketAsyncEventArgs args)
        {
            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                OnError(args);
                return false;
            }

            var dataCopy = new byte[transferred];
            Buffer.BlockCopy(args.Buffer, 0, dataCopy, 0, transferred);
            var eventArgs = new DataArrivedEventArgs(dataCopy);

            DataArrivedInternal.Invoke(this, eventArgs);

            return true;
        }

        #endregion

        /// <inheritdoc />
        protected override void OnClosed()
        {
            DataArrivedInternal = null;
            ClearBuffer();
        }
    }
}
