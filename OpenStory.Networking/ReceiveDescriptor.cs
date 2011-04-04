using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an asynchronous network receive buffer.
    /// </summary>
    sealed class ReceiveDescriptor : Descriptor
    {
        /// <summary>
        /// The default buffer size, set to match the AesTransform block size.
        /// </summary>
        private const int BufferSize = 1460;

        /// <summary>
        /// The event used to handle incoming data.
        /// </summary>
        public event EventHandler<DataArrivedEventArgs> OnDataArrived
        {
            add
            {
                if (OnDataArrivedInternal != null)
                {
                    throw new InvalidOperationException("The event should not have more than one subscriber.");
                }
                this.OnDataArrivedInternal += value;
            }
            remove
            {
                if (OnDataArrivedInternal == null)
                {
                    throw new InvalidOperationException("The event has no subscribers.");
                }
                this.OnDataArrivedInternal += value;
            }
        }

        private event EventHandler<DataArrivedEventArgs> OnDataArrivedInternal;

        private byte[] receiveBuffer;

        /// <summary>
        /// Initializes a new instance of ReceiveDescriptor.
        /// </summary>
        /// <param name="container">The <see cref="IDescriptorContainer"/> for the new instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="container"/> is <c>null</c>.
        /// </exception>
        public ReceiveDescriptor(IDescriptorContainer container)
            : base(container)
        {
            base.SocketArgs.Completed += this.EndReceiveAsynchronous;

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
            base.SocketArgs.SetBuffer(null, 0, 0);
        }

        private void ResetPositions()
        {
            base.SocketArgs.SetBuffer(this.receiveBuffer, 0, BufferSize);
        }

        #endregion

        #region Async receive methods

        /// <summary>
        /// Starts the receive process.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="OnDataArrived"/> event has no subscibers.
        /// </exception>
        public void StartReceive()
        {
            if (this.OnDataArrivedInternal == null)
            {
                throw new InvalidOperationException("OnDataArrived has no subscribers.");
            }

            this.SetFreshBuffer();

            this.BeginReceive();
        }

        private void BeginReceive()
        {
            if (!base.Container.IsActive) return;

            try
            {
                // For the confused: ReceiveAsync will return false if
                // the operation completed synchronously.
                // As long as that is happening, this loop will handle
                // the data transfer synchronously as well.
                while (!base.Container.Socket.ReceiveAsync(base.SocketArgs))
                {
                    if (!this.EndReceiveSynchronous(base.SocketArgs)) break;
                }
            }
            catch (ObjectDisposedException)
            {
                base.Container.Close();
            }
        }

        /// <summary>
        /// Synchronous EndReceive.
        /// </summary>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><c>true</c> if there is more data to send; otherwise, <c>false</c>.</returns>
        private bool EndReceiveSynchronous(SocketAsyncEventArgs args)
        {
            return this.HandleTransferredData(args);
        }

        /// <summary>
        /// Asynchronous EndReceive method, also the callback for the Completed event.
        /// </summary>
        /// <param name="sender">The sender of the Completed event.</param>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        private void EndReceiveAsynchronous(object sender, SocketAsyncEventArgs args)
        {
            if (!HandleTransferredData(args)) return;

            this.BeginReceive();
        }
        
        /// <summary>
        /// Handles the transferred data for the operation.
        /// </summary>
        /// <remarks>
        /// This method returns <c>false</c> on connection errors.
        /// </remarks>
        /// <param name="args">The SocketAsyncEventArgs object for this operation.</param>
        /// <returns><c>true</c> if there is more data to send; otherwise, <c>false</c>.</returns>
        private bool HandleTransferredData(SocketAsyncEventArgs args)
        {
            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                base.HandleError(args);
                return false;
            }

            byte[] dataCopy = new byte[transferred];
            Buffer.BlockCopy(args.Buffer, 0, dataCopy, 0, transferred);
            var eventArgs = new DataArrivedEventArgs(dataCopy);

            this.OnDataArrivedInternal.Invoke(this, eventArgs);

            return true;
        }

        #endregion

        protected override void CloseImpl()
        {
            this.OnDataArrivedInternal = null;
            this.ClearBuffer();
        }
    }
}