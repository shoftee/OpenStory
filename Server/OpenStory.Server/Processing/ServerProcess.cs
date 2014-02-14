using System;
using System.ComponentModel;
using Ninject.Extensions.Logging;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
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
        private readonly IServerSessionFactory sessionFactory;
        private readonly ISocketAcceptorFactory socketAcceptorFactory;
        private readonly IPacketScheduler packetScheduler;
        private readonly IRollingIvFactoryProvider rollingIvFactoryProvider;
        private readonly IvGenerator ivGenerator;
        private readonly ILogger logger;

        private ServerConfiguration serverConfiguration;

        private SocketAcceptor acceptor;
        private RollingIvFactory ivFactory;
        private bool isDisposed;

        /// <inheritdoc/>
        public event EventHandler<ServerSessionEventArgs> ConnectionOpened;

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

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
        }

        /// <inheritdoc/>
        public void Configure(OsServiceConfiguration configuration)
        {
            this.ThrowIfRunning();

            this.serverConfiguration = new ServerConfiguration(configuration);

            this.ConfigureInternal();
        }

        private void ConfigureInternal()
        {
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
            this.ThrowIfRunning();

            this.IsRunning = true;

            this.logger.Info(@"Now listening on port {0}.", this.acceptor.Endpoint.Port);
            this.acceptor.Start();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            this.ThrowIfNotRunning();

            this.logger.Info(@"Now shutting down...");
            this.acceptor.Stop();

            this.IsRunning = false;
        }

        private void OnSocketAccepted(object sender, SocketEventArgs e)
        {
            var session = this.sessionFactory.CreateSession();
            this.packetScheduler.Register(session);
            session.AttachSocket(e.Socket);
            this.OnConnectionOpened(session);

            this.StartSession(session);
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

        /// <summary>
        /// Checks if the <see cref="IsRunning"/> property is true and throws an exception if it is not.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is not running.
        /// </exception>
        private void ThrowIfNotRunning()
        {
            if (!this.IsRunning)
            {
                throw new InvalidOperationException(ServerStrings.ServerNotRunning);
            }
        }

        /// <summary>
        /// Checks if the <see cref="IsRunning"/> property is true and throws and exception if it is.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is running.
        /// </exception>
        private void ThrowIfRunning()
        {
            if (this.IsRunning)
            {
                throw new InvalidOperationException(ServerStrings.ServerAlreadyRunning);
            }
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