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
            add { _receiveDescriptor.DataArrived += value; }
            remove { _receiveDescriptor.DataArrived -= value; }
        }

        /// <summary>
        /// Occurs when the <see cref="NetworkSession" /> begins closing.
        /// </summary>
        public event EventHandler<ConnectionClosingEventArgs> Closing;

        /// <summary>
        /// Occurs when a connection error occurs.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> SocketError
        {
            add
            {
                _sendDescriptor.Error += value;
                _receiveDescriptor.Error += value;
            }
            remove
            {
                _sendDescriptor.Error -= value;
                _receiveDescriptor.Error -= value;
            }
        }

        #endregion

        #region Fields and properties

        /// <summary>
        /// Gets whether the socket is currently disconnected or not.
        /// </summary>
        private readonly AtomicBoolean _isActive;

        private ReceiveDescriptor _receiveDescriptor;
        private SendDescriptor _sendDescriptor;
        private Socket _socket;

        /// <inheritdoc/>
        public bool IsActive => _isActive.Value;

        /// <summary>
        /// Gets the remote endpoint of the session.
        /// </summary>
        public IPEndPoint RemoteEndpoint
        {
            get
            {
                if (_socket == null)
                {
                    return null;
                }

                try
                {
                    var endpoint = _socket.RemoteEndPoint as IPEndPoint;
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
            _isActive = false;

            _receiveDescriptor = new ReceiveDescriptor(this);
            _sendDescriptor = new SendDescriptor(this);
        }

        /// <summary>
        /// Attaches a socket to this <see cref="NetworkSession"/>.
        /// </summary>
        /// <param name="sessionSocket">The underlying socket to use for this session.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="NetworkSession"/> instance already has a socket attached to it.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="sessionSocket" /> is <see langword="null"/>.
        /// </exception>
        public void AttachSocket(Socket sessionSocket)
        {
            Guard.NotNull(() => sessionSocket, sessionSocket);

            if (_socket != null)
            {
                throw new InvalidOperationException(CommonStrings.SessionSocketAlreadyAttached);
            }

            _socket = sessionSocket;
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
            if (_socket == null)
            {
                throw new InvalidOperationException(CommonStrings.NoSocketAttached);
            }

            if (!_isActive.FlipIf(false))
            {
                throw new InvalidOperationException(CommonStrings.SessionAlreadyActive);
            }

            _receiveDescriptor.StartReceive();
        }

        /// <inheritdoc />
        public void Close(string reason)
        {
            Closing?.Invoke(this, new ConnectionClosingEventArgs(reason));

            Closing = null;
            if (!_isActive.FlipIf(true))
            {
                return;
            }

            _socket.Close();

            _receiveDescriptor.Close();
            _sendDescriptor.Close();
        }

        /// <summary>
        /// Writes a byte array to the network stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        public void Write(byte[] data)
        {
            lock (_sendDescriptor)
            {
                _sendDescriptor.Write(data);
            }
        }

        #endregion

        #region Explicitly implemented members of IDescriptorContainer

        /// <inheritdoc />
        Socket IDescriptorContainer.Socket => _socket;

        /// <inheritdoc />
        bool IDescriptorContainer.IsActive => _isActive.Value;

        /// <inheritdoc />
        void IDescriptorContainer.Close(string reason)
        {
            Close(reason);
        }

        #endregion

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Misc.AssignNullAndDispose(ref _socket);
            Misc.AssignNullAndDispose(ref _sendDescriptor);
            Misc.AssignNullAndDispose(ref _receiveDescriptor);
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            var endpoint = RemoteEndpoint;
            var endpointString = endpoint?.ToString() ?? @"None";
            return $"Endpoint: {endpointString}";
        }
    }
}
