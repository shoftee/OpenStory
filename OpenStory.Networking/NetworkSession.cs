using System;
using System.Net.Sockets;
using OpenStory.Common;
using OpenStory.Common.Tools;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a network session used for sending and receiving data.
    /// </summary>
    public abstract class NetworkSession : IDescriptorContainer
    {
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        /// <summary>
        /// The event is raised just before the NetworkSession is closed.
        /// </summary>
        public event EventHandler OnClose;

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

        #region Fields and properties

        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;

        /// <summary>
        /// Gets the unique ID for the NetworkSession.
        /// </summary>
        public int SessionId { get; private set; }

        /// <summary>
        /// Gets whether the socket is currently disconnected or not.
        /// </summary>
        /// <remarks>
        /// The condition for the name is inverted because connected-ness is common.
        /// </remarks>
        protected AtomicBoolean IsDisconnected { get; private set; }

        /// <summary>
        /// Gets the socket being used for this session.
        /// </summary>
        public Socket Socket { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the NetworkSession class.
        /// </summary>
        /// <param name="socket">The underlying socket for the NetworkSession.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket" /> is <c>null</c>.
        /// </exception>
        protected NetworkSession(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            this.receiveDescriptor = new ReceiveDescriptor(this);
            this.sendDescriptor = new SendDescriptor(this);

            this.Socket = socket;

            this.IsDisconnected = new AtomicBoolean(false);
            this.SessionId = RollingSessionId.Increment();
        }

        #region Connection methods

        /// <summary>
        /// Starts the receive process.
        /// </summary>
        public void Start()
        {
            this.StartImpl();
            this.receiveDescriptor.StartReceive();
        }

        /// <summary>
        /// A hook to the beginning of the 
        /// publicly exposed <see cref="Start()"/> method.
        /// </summary>
        protected abstract void StartImpl();

        /// <summary>
        /// Releases the session so it can be reused with a new socket.
        /// </summary>
        public void Close()
        {
            if (this.OnClose != null)
            {
                this.OnClose(this, EventArgs.Empty);
            }

            this.OnClose = null;
            if (this.IsDisconnected.CompareExchange(false, true))
            {
                return;
            }

            this.Socket.Dispose();

            this.receiveDescriptor.Close();
            this.sendDescriptor.Close();

            this.CloseImpl();
        }

        /// <summary>
        /// A hook to the end of the publicly 
        /// exposed <see cref="Close()"/> method.
        /// </summary>
        protected abstract void CloseImpl();

        #endregion

        /// <summary>
        /// Writes a byte array to the network stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        protected void Write(byte[] data)
        {
            lock (sendDescriptor)
            {
                this.sendDescriptor.Write(data);
            }
        }

        /// <summary>
        /// Processes a received data segment.
        /// </summary>
        /// <param name="data">The data segment to process.</param>
        protected abstract void HandleIncomingData(byte[] data);

        #region IDescriptorContainer explicit implementations

        bool IDescriptorContainer.IsDisconnected
        {
            get { return this.IsDisconnected.Value; }
        }

        void IDescriptorContainer.Close()
        {
            this.Close();
        }

        #endregion
    }
}