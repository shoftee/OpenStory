using System;
using System.Net.Sockets;
using OpenStory.Common;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a network session used for sending and receiving data.
    /// </summary>
    public sealed class NetworkSession : IDescriptorContainer
    {
        /// <summary>
        /// Initializes a new instance of the NetworkSession class.
        /// </summary>
        /// <param name="socket">The underlying socket for the NetworkSession.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket" /> is <c>null</c>.
        /// </exception>
        public NetworkSession(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            this.receiveDescriptor = new ReceiveDescriptor(this);
            this.sendDescriptor = new SendDescriptor(this);

            this.Socket = socket;

            this.isActive = new AtomicBoolean(false);
        }

        #region Connection methods

        /// <summary>
        /// Starts the receive process.
        /// </summary>
        public void Start()
        {
            if (this.isActive.CompareExchange(comparand: false, newValue: true))
            {
                throw new InvalidOperationException("This session is already active.");
            }

            this.receiveDescriptor.StartReceive();
        }

        /// <summary>
        /// Releases the session so it can be reused with a new socket.
        /// </summary>
        public void Close()
        {
            if (this.OnClosing != null)
            {
                this.OnClosing(this, EventArgs.Empty);
            }

            this.OnClosing = null;
            if (!this.isActive.CompareExchange(comparand: true, newValue: false))
            {
                return;
            }

            this.Socket.Dispose();

            this.receiveDescriptor.Close();
            this.sendDescriptor.Close();
        }

        #endregion

        /// <summary>
        /// The event is raised just before the NetworkSession is closed.
        /// </summary>
        public event EventHandler OnClosing;

        /// <summary>
        /// The event is raised when a data segment arrives.
        /// </summary>
        /// <remarks>
        /// This event doesn't support more than 1 subscriber.
        /// Attempts to subscribe more than 1 method to this event 
        /// will throw an <see cref="InvalidOperationException"/>.
        /// </remarks>
        public event EventHandler<DataArrivedEventArgs> OnDataArrived
        {
            add { this.receiveDescriptor.OnDataArrived += value; }
            remove { this.receiveDescriptor.OnDataArrived -= value; }
        }

        /// <summary>
        /// The event used to handle socket errors.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> OnError
        {
            add
            {
                this.sendDescriptor.OnError += value;
                this.receiveDescriptor.OnError += value;
            }
            remove
            {
                this.sendDescriptor.OnError -= value;
                this.receiveDescriptor.OnError -= value;
            }
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

        #region IDescriptorContainer explicit implementations

        bool IDescriptorContainer.IsActive
        {
            get { return this.isActive.Value; }
        }

        void IDescriptorContainer.Close()
        {
            this.Close();
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// Gets whether the socket is currently disconnected or not.
        /// </summary>
        private AtomicBoolean isActive;

        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;

        /// <summary>
        /// Gets the socket being used for this session.
        /// </summary>
        public Socket Socket { get; private set; }

        #endregion
    }
}