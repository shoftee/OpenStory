using System;
using System.Net.Sockets;

namespace OpenMaple.Networking
{
    sealed class ReceiveDescriptor
    {
        public event EventHandler<SocketErrorEventArgs> OnError;

        /// <summary>
        /// The event used to handle incoming data.
        /// </summary>
        public event OnDataDelegate OnData
        {
            add
            {
                if (OnDataInternal != null) throw new InvalidOperationException("OnData can't have more than one subscriber.");
                OnDataInternal += value;
            }
            remove
            {
                if (OnDataInternal == null) throw new InvalidOperationException("OnData has no subscribers.");
                OnDataInternal -= value;
            }
        }

        private event OnDataDelegate OnDataInternal;

        private IDescriptorContainer container;

        private SocketAsyncEventArgs socketArgs;

        private ArraySegment<byte> buffer;
        private int start;
        private int remaining;

        public ReceiveDescriptor(IDescriptorContainer container)
        {
            this.container = container;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += this.EndReceive;

            this.ClearBuffer();
        }

        public void SetBuffer(ArraySegment<byte> newBuffer)
        {
            this.buffer = newBuffer;
            this.socketArgs.SetBuffer(this.buffer.Array, this.buffer.Offset, this.buffer.Offset);
            this.ResetPositions();
        }

        private void ClearBuffer()
        {
            this.buffer = default(ArraySegment<byte>);
            this.ResetPositions();
        }

        public void StartReceive()
        {
            if (OnDataInternal == null)
            {
                throw new InvalidOperationException("OnData needs to have a subscriber.");
            }
            if (this.buffer.Array == null)
            {
                throw new InvalidOperationException("Buffer not set, call SetBuffer for this descriptor before you use it.");
            }

            this.BeginReceive();
        }

        private void ResetPositions(bool resetBufferPositions = false)
        {
            this.start = this.buffer.Offset;
            this.remaining = this.buffer.Count;
            if (resetBufferPositions)
            {
                this.socketArgs.SetBuffer(this.start, this.remaining);
            }
        }

        private void BeginReceive()
        {
            if (container.IsDisconnected) return;

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
            if (container.IsDisconnected) return;

            int transferred = args.BytesTransferred;
            if (transferred <= 0)
            {
                if (args.SocketError != SocketError.Success &&
                    args.SocketError != SocketError.ConnectionReset &&
                    this.OnError != null)
                {
                    this.OnError(this, new SocketErrorEventArgs(args.SocketError));
                }
                this.container.Close();
                return;
            }

            byte[] receivedBlock = new byte[transferred];
            Buffer.BlockCopy(this.buffer.Array, this.start, receivedBlock, 0, transferred);
            this.OnDataInternal(receivedBlock);

            this.start += transferred;
            this.remaining -= transferred;

            if (this.remaining == 0)
            {
                ResetPositions(true);
            }
        }

        public void Close()
        {
            this.socketArgs.SetBuffer(null, 0, 0);
            this.ClearBuffer();
        }
    }
}
