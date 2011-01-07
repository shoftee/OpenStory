using System;
using System.Net.Sockets;
using OpenMaple.Threading;
using OpenMaple.Tools;

namespace OpenMaple.Networking
{
    sealed partial class NetworkSession
    {
        private sealed class ReceiveDescriptor
        {
            private NetworkSession container;
            private Socket Socket { get { return this.container.socketInternal; } }
            private AtomicBoolean IsDisconnected { get { return this.container.isDisconnected; } }

            private SocketAsyncEventArgs socketArgs;

            private ArraySegment<byte> buffer;
            private int start;
            private int remaining;

            public ReceiveDescriptor(NetworkSession container)
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
                if (this.buffer.Array == null)
                {
                    throw GetBufferNotSetException();
                }

                this.BeginReceive();
            }

            private void ResetPositions(bool resetBufferPositions = false) {
                this.start = this.buffer.Offset;
                this.remaining = this.buffer.Count;
                if (resetBufferPositions)
                {
                    this.socketArgs.SetBuffer(this.start, this.remaining);
                }
            }

            private void BeginReceive()
            {
                if (this.IsDisconnected.Value) return;

                try
                {
                    if (!this.Socket.ReceiveAsync(this.socketArgs))
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
                if (this.IsDisconnected.Value) return;

                int transferred = args.BytesTransferred;
                if (transferred <= 0)
                {
                    if (args.SocketError != SocketError.Success &&
                        args.SocketError != SocketError.ConnectionReset)
                    {
                        Log.WriteError("Socket({0}) error: {1}", this.container.RemoteAddress, args.SocketError);
                    }
                    this.container.Close();
                    return;
                }

                byte[] receivedBlock = new byte[transferred];
                Buffer.BlockCopy(this.buffer.Array, this.start, receivedBlock, 0, transferred);
                this.container.ReceiveData(receivedBlock);

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
}
