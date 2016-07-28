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
        private readonly AtomicBoolean isRunning;
        private bool isDisposed;
        private bool isConfigured;

        private readonly IServerSessionFactory sessionFactory;
        private readonly ISocketAcceptorFactory socketAcceptorFactory;
        private readonly IPacketScheduler packetScheduler;
        private readonly IRollingIvFactoryProvider rollingIvFactoryProvider;
        private readonly IvGenerator ivGenerator;
        private readonly ILogger logger;

        private ServerConfiguration serverConfiguration;

        private SocketAcceptor acceptor;
        private RollingIvFactory ivFactory;

        /// <inheritdoc/>
        public event EventHandler<ServerSessionEventArgs> ConnectionOpened;

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning => this.isRunning.Value;

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
            this.sessionFactory = sessionFactory;
            this.socketAcceptorFactory = socketAcceptorFactory;
            this.packetScheduler = packetScheduler;
            this.rollingIvFactoryProvider = rollingIvFactoryProvider;
            this.ivGenerator = ivGenerator;
            this.logger = logger;

            this.isConfigured = false;
            this.isDisposed = false;
            this.isRunning = new AtomicBoolean(false);
        }

        /// <inheritdoc/>
        public void Configure(OsServiceConfiguration configuration)
        {
            if (this.IsRunning)
            {
                throw GetServerAlreadyRunningException();
            }

            this.ConfigureInternal(configuration);

            this.isConfigured = true;
            if (this.serverConfiguration.Subversion == "")
            {
                this.logger.Info(
                    @"Configured as version {0}, locale {1}.", 
                    this.serverConfiguration.Version,
                    this.serverConfiguration.LocaleId);
            }
            else
            {
                this.logger.Info(
                    @"Configured as version {0}.{1}, locale {2}.", 
                    this.serverConfiguration.Version,
                    this.serverConfiguration.Subversion,
                    this.serverConfiguration.LocaleId);
            }
        }

        private void ConfigureInternal(OsServiceConfiguration configuration)
        {
            this.serverConfiguration = new ServerConfiguration(configuration);

            this.ivFactory = this.CreateRollingIvFactory();
            this.acceptor = this.CreateSocketAcceptor();
        }

        private RollingIvFactory CreateRollingIvFactory()
        {
            var version = this.serverConfiguration.Version;

            var rollingIvFactory = this.rollingIvFactoryProvider.CreateFactory(version);

            return rollingIvFactory;
        }

        private SocketAcceptor CreateSocketAcceptor()
        {
            var endpoint = this.serverConfiguration.Endpoint;

            var socketAcceptor = this.socketAcceptorFactory.CreateSocketAcceptor(endpoint);
            socketAcceptor.SocketAccepted += this.OnSocketAccepted;

            return socketAcceptor;
        }

        /// <inheritdoc/>
        public void Start()
        {
            if (!this.isConfigured)
            {
                throw GetServerIsNotConfiguredException();
            }

            if (!this.isRunning.FlipIf(false))
            {
                throw GetServerAlreadyRunningException();
            }

            this.acceptor.Start();
            this.logger.Info(@"Now listening on port {0}.", this.acceptor.Endpoint.Port);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            if (this.isRunning.FlipIf(true))
            {
                this.logger.Info(@"Now shutting down...");
                this.acceptor.Stop();
            }
        }

        private void OnSocketAccepted(object sender, SocketEventArgs e)
        {
            var sessionSocket = e.Socket;
            var session = this.CreateServerSession(sessionSocket);

            this.packetScheduler.Register(session);
            this.OnConnectionOpened(session);

            this.logger.Debug(@"Accepted session #{0}: {1}", session.NetworkSessionId, session);

            this.StartSession(session);
        }

        private IServerSession CreateServerSession(Socket sessionSocket)
        {
            var session = this.sessionFactory.CreateSession();
            session.SocketError += this.OnSessionSocketError;
            session.Closing += OnSessionClosing;
            session.AttachSocket(sessionSocket);
            return session;
        }

        private void OnSessionClosing(object sender, ConnectionClosingEventArgs args)
        {
            var session = (IServerSession)sender;
            this.logger.Debug(@"Session #{0} closing: {1}", session.NetworkSessionId, args.Reason);
        }

        private void OnSessionSocketError(object sender, SocketErrorEventArgs args)
        {
            var session = (IServerSession)sender;
            this.logger.Debug(@"Session #{0} error: {1}", session.NetworkSessionId, args.Error);
        }

        private void StartSession(IServerSession session)
        {
            var clientIv = this.ivGenerator.GetNewIv();
            var serverIv = this.ivGenerator.GetNewIv();

            var info = new ConfiguredHandshakeInfo(this.serverConfiguration, clientIv, serverIv);
            var crypto = EndpointCrypto.Server(this.ivFactory, clientIv, serverIv);
            session.Start(crypto, info);
        }

        private void OnConnectionOpened(IServerSession session)
        {
            var handler = this.ConnectionOpened;
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
            if (!this.isDisposed)
            {
                if (this.acceptor != null)
                {
                    this.acceptor.Dispose();
                }

                this.isDisposed = true;
            }
        }

        #endregion
    }
}