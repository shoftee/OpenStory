using System;
using System.ComponentModel;
using System.Net.Sockets;
using Ninject.Extensions.Logging;
using OpenStory.Common;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Networking;
using OpenStory.Server.Networking;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a process which handles network communication.
    /// </summary>
    [Localizable(true)]
    public sealed class ServerProcess : IServerProcess, IDisposable
    {
        private readonly AtomicBoolean _isRunning;
        private bool _isDisposed;
        private bool _isConfigured;

        private readonly IServerSessionFactory _sessionFactory;
        private readonly ISocketAcceptorFactory _socketAcceptorFactory;
        private readonly IPacketScheduler _packetScheduler;
        private readonly IRollingIvFactoryProvider _rollingIvFactoryProvider;
        private readonly IvGenerator _ivGenerator;
        private readonly ILogger _logger;

        private ServerConfiguration _serverConfiguration;

        private SocketAcceptor _acceptor;
        private RollingIvFactory _ivFactory;

        /// <inheritdoc/>
        public event EventHandler<ServerSessionEventArgs> ConnectionOpened;

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning => _isRunning.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerProcess"/> class.
        /// </summary>
        public ServerProcess(
            IServerSessionFactory sessionFactory,
            ISocketAcceptorFactory socketAcceptorFactory,
            IPacketScheduler packetScheduler,
            IRollingIvFactoryProvider rollingIvFactoryProvider,
            IvGenerator ivGenerator,
            ILogger logger)
        {
            _sessionFactory = sessionFactory;
            _socketAcceptorFactory = socketAcceptorFactory;
            _packetScheduler = packetScheduler;
            _rollingIvFactoryProvider = rollingIvFactoryProvider;
            _ivGenerator = ivGenerator;
            _logger = logger;

            _isConfigured = false;
            _isDisposed = false;
            _isRunning = new AtomicBoolean(false);
        }

        /// <inheritdoc/>
        public void Configure(OsServiceConfiguration configuration)
        {
            if (IsRunning)
            {
                throw GetServerAlreadyRunningException();
            }

            ConfigureInternal(configuration);

            _isConfigured = true;
            if (_serverConfiguration.Subversion == "")
            {
                _logger.Info(
                    @"Configured as version {0}, locale {1}.",
                    _serverConfiguration.Version,
                    _serverConfiguration.LocaleId);
            }
            else
            {
                _logger.Info(
                    @"Configured as version {0}.{1}, locale {2}.",
                    _serverConfiguration.Version,
                    _serverConfiguration.Subversion,
                    _serverConfiguration.LocaleId);
            }
        }

        private void ConfigureInternal(OsServiceConfiguration configuration)
        {
            _serverConfiguration = new ServerConfiguration(configuration);

            _ivFactory = CreateRollingIvFactory();
            _acceptor = CreateSocketAcceptor();
        }

        private RollingIvFactory CreateRollingIvFactory()
        {
            var version = _serverConfiguration.Version;

            var rollingIvFactory = _rollingIvFactoryProvider.CreateFactory(version);

            return rollingIvFactory;
        }

        private SocketAcceptor CreateSocketAcceptor()
        {
            var endpoint = _serverConfiguration.Endpoint;

            var socketAcceptor = _socketAcceptorFactory.CreateSocketAcceptor(endpoint);
            socketAcceptor.SocketAccepted += OnSocketAccepted;

            return socketAcceptor;
        }

        /// <inheritdoc/>
        public void Start()
        {
            if (!_isConfigured)
            {
                throw GetServerIsNotConfiguredException();
            }

            if (!_isRunning.FlipIf(false))
            {
                throw GetServerAlreadyRunningException();
            }

            _acceptor.Start();
            _logger.Info(@"Now listening on port {0}.", _acceptor.Endpoint.Port);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            if (_isRunning.FlipIf(true))
            {
                _logger.Info(@"Now shutting down...");
                _acceptor.Stop();
            }
        }

        private void OnSocketAccepted(object sender, SocketEventArgs e)
        {
            var sessionSocket = e.Socket;
            var session = CreateServerSession(sessionSocket);

            _packetScheduler.Register(session);
            OnConnectionOpened(session);

            _logger.Debug(@"Accepted session #{0}: {1}", session.NetworkSessionId, session);

            StartSession(session);
        }

        private IServerSession CreateServerSession(Socket sessionSocket)
        {
            var session = _sessionFactory.CreateSession();
            session.SocketError += OnSessionSocketError;
            session.Closing += OnSessionClosing;
            session.AttachSocket(sessionSocket);
            return session;
        }

        private void OnSessionClosing(object sender, ConnectionClosingEventArgs args)
        {
            var session = (IServerSession)sender;
            _logger.Debug(@"Session #{0} closing: {1}", session.NetworkSessionId, args.Reason);
        }

        private void OnSessionSocketError(object sender, SocketErrorEventArgs args)
        {
            var session = (IServerSession)sender;
            _logger.Debug(@"Session #{0} error: {1}", session.NetworkSessionId, args.Error);
        }

        private void StartSession(IServerSession session)
        {
            var clientIv = _ivGenerator.GetNewIv();
            var serverIv = _ivGenerator.GetNewIv();

            var info = new ConfiguredHandshakeInfo(_serverConfiguration, clientIv, serverIv);
            var crypto = EndpointCrypto.Server(_ivFactory, clientIv, serverIv);
            session.Start(crypto, info);
        }

        private void OnConnectionOpened(IServerSession session)
        {
            var handler = ConnectionOpened;
            if (handler != null)
            {
                var args = new ServerSessionEventArgs(session);
                handler.Invoke(this, args);
            }
        }

        #region Exception methods

        private static InvalidOperationException GetServerIsNotConfiguredException()
        {
            return new InvalidOperationException(ServerStrings.ServerIsNotConfigured);
        }

        private static InvalidOperationException GetServerAlreadyRunningException()
        {
            return new InvalidOperationException(ServerStrings.ServerAlreadyRunning);
        }

        #endregion

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_acceptor != null)
                {
                    _acceptor.Dispose();
                }

                _isDisposed = true;
            }
        }

        #endregion
    }
}