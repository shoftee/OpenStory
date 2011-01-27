using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    sealed class ReceiveDescriptor : Descriptor
    {
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
            base.SocketArgs.Completed += this.EndReceive;

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
            if (base.Container.IsDisconnected) return;

            try
            {
                if (!base.Container.Socket.ReceiveAsync(base.SocketArgs))
                {
                    this.EndReceive(null, base.SocketArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                base.Container.Close();
            }
        }

        private void EndReceive(object sender, SocketAsyncEventArgs args)
        {
            if (base.Container.IsDisconnected) return;

            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                base.RaiseErrorEvent(args);
                return;
            }

            byte[] dataCopy = new byte[transferred];
            Buffer.BlockCopy(args.Buffer, 0, dataCopy, 0, transferred);
            var eventArgs = new DataArrivedEventArgs(dataCopy);

            this.OnDataArrivedInternal.Invoke(this, eventArgs);

            this.BeginReceive();
        }

        #endregion

        protected override void CloseImpl()
        {
            this.OnDataArrivedInternal = null;
            this.ClearBuffer();
        }
    }
}