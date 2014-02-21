using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a network session used for sending and receiving data.
    /// </summary>
    [Localizable(true)]
    public sealed class NetworkSession : IDescriptorContainer, IDisposable
    {
        #region Events

        /// <summary>
        /// Occurs when a data segment arrives.
        /// </summary>
        /// <remarks>
        /// This event doesn't support more than one subscriber.
        /// Attempts to subscribe more than one method to this event 
        /// will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        public event EventHandler<DataArrivedEventArgs> DataArrived
        {
            add { this.receiveDescriptor.DataArrived += value; }
            remove { this.receiveDescriptor.DataArrived -= value; }
        }

        /// <summary>
        /// Occurs when the <see cref="NetworkSession" /> begins closing.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Occurs when a connection error occurs.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> SocketError
        {
            add
            {
                this.sendDescriptor.Error += value;
                this.receiveDescriptor.Error += value;
            }
            remove
            {
                this.sendDescriptor.Error -= value;
                this.receiveDescriptor.Error -= value;
            }
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// Gets whether the socket is currently disconnected or not.
        /// </summary>
        private readonly AtomicBoolean isActive;

        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;
        private Socket socket;

        /// <inheritdoc />
        public Socket Socket
        {
            get { return this.socket; }
        }

        /// <summary>
        /// Gets the remote endpoint of the session.
        /// </summary>
        public IPEndPoint RemoteEndpoint
        {
            get
            {
                if (this.Socket == null)
                {
                    return null;
                }

                try
                {
                    var endpoint = this.Socket.RemoteEndPoint as IPEndPoint;
                    return endpoint;
                }
                catch (ObjectDisposedException)
                {
                    return null;
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkSession"/> class.
        /// </summary>
        public NetworkSession()
        {
            this.isActive = false;

            this.receiveDescriptor = new ReceiveDescriptor(this);
            this.sendDescriptor = new SendDescriptor(this);
        }

        /// <summary>
        /// Attaches a socket to this <see cref="NetworkSession"/>.
        /// </summary>
        /// <param name="socket">The underlying socket to use.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="NetworkSession"/> instance already has a socket attached to it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket" /> is <see langword="null"/>.
        /// </exception>
        public void AttachSocket(Socket socket)
        {
            Guard.NotNull(() => socket, socket);

            if (this.Socket != null)
            {
                throw new InvalidOperationException(CommonStrings.SessionSocketAlreadyAttached);
            }

            this.socket = socket;
        }

        #endregion

        #region Socket operations

        /// <summary>
        /// Starts the receive process.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="NetworkSession"/> instance does not have a socket attached to it or 
        /// if the instance is already active.
        /// </exception>
        public void Start()
        {
            if (this.Socket == null)
            {
                throw new InvalidOperationException(CommonStrings.NoSocketAttached);
            }

            if (!this.isActive.FlipIf(false))
            {
                throw new InvalidOperationException(CommonStrings.SessionAlreadyActive);
            }

            this.receiveDescriptor.StartReceive();
        }

        /// <inheritdoc />
        public void Close()
        {
            if (this.Closing != null)
            {
                this.Closing(this, EventArgs.Empty);
            }

            this.Closing = null;
            if (!this.isActive.FlipIf(true))
            {
                return;
            }

            this.Socket.Close();

            this.receiveDescriptor.Close();
            this.sendDescriptor.Close();
        }

        /// <summary>
        /// Writes a byte array to the network stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        public void Write(byte[] data)
        {
            lock (this.sendDescriptor)
            {
                this.sendDescriptor.Write(data);
            }
        }

        #endregion

        #region Explicitly implemented members of IDescriptorContainer

        /// <inheritdoc />
        bool IDescriptorContainer.IsActive
        {
            get { return this.isActive.Value; }
        }

        /// <inheritdoc />
        void IDescriptorContainer.Close()
        {
            this.Close();
        }

        #endregion

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Misc.AssignNullAndDispose(ref this.socket);
            Misc.AssignNullAndDispose(ref this.sendDescriptor);
            Misc.AssignNullAndDispose(ref this.receiveDescriptor);
        }

        #endregion
    }
}
