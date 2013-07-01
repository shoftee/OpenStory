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
        /// The event is raised when a data segment arrives.
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
        /// The event is raised when the <see cref="NetworkSession" /> begins closing.
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// The event is raised when a connection error occurs.
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

        private readonly ReceiveDescriptor receiveDescriptor;
        private readonly SendDescriptor sendDescriptor;

        /// <inheritdoc />
        public Socket Socket { get; private set; }

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
        /// Thrown if <paramref name="socket" /> is <c>null</c>.
        /// </exception>
        public void AttachSocket(Socket socket)
        {
            if (this.Socket != null)
            {
                throw new InvalidOperationException(Exceptions.SessionSocketAlreadyAttached);
            }

            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }

            this.Socket = socket;
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
                throw new InvalidOperationException(Exceptions.NoSocketAttached);
            }

            if (this.isActive.CompareExchange(comparand: false, newValue: true))
            {
                throw new InvalidOperationException(Exceptions.SessionAlreadyActive);
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
            if (!this.isActive.CompareExchange(comparand: true, newValue: false))
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
            if (this.Socket != null)
            {
                this.Socket.Dispose();
                this.Socket = null;
            }

            if (this.sendDescriptor != null)
            {
                this.sendDescriptor.Dispose();
            }

            if (this.receiveDescriptor != null)
            {
                this.receiveDescriptor.Dispose();
            }
        }

        #endregion
    }
}
