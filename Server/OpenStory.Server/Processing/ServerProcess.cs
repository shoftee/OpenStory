using System;
using System.ComponentModel;
using Ninject.Extensions.Logging;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Networking;
using OpenStory.Services;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a process which handles network communication.
    /// </summary>
    [Localizable(true)]
    internal sealed class ServerProcess : IServerProcess, IDisposable
    {
        private readonly IServerSessionFactory sessionFactory;
        private readonly ISocketAcceptorFactory socketAcceptorFactory;
        private readonly IPacketScheduler packetScheduler;
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
        /// <param name="sessionFactory">The <see cref="IServerSessionFactory"/> for this server.</param>
        /// <param name="socketAcceptorFactory">The <see cref="ISocketAcceptorFactory"/> for this server.</param>
        /// <param name="packetScheduler">The <see cref="IPacketScheduler"/> for this server.</param>
        /// <param name="ivGenerator">The <see cref="IvGenerator"/> for this server.</param>
        /// <param name="logger">The logger to use for this server.</param>
        public ServerProcess(IServerSessionFactory sessionFactory, ISocketAcceptorFactory socketAcceptorFactory, IPacketScheduler packetScheduler, IvGenerator ivGenerator, ILogger logger)
        {
            this.sessionFactory = sessionFactory;
            this.socketAcceptorFactory = socketAcceptorFactory;
            this.packetScheduler = packetScheduler;
            this.ivGenerator = ivGenerator;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public void Configure(ServiceConfiguration configuration)
        {
            this.ThrowIfRunning();

            this.serverConfiguration = new ServerConfiguration(configuration);

            this.ConfigureInternal();
        }

        private void ConfigureInternal()
        {
            this.ivFactory = IvFactories.GetEmsFactory(this.serverConfiguration.Version);

            this.acceptor = socketAcceptorFactory.CreateSocketAcceptor(this.serverConfiguration.Endpoint);
            this.acceptor.SocketAccepted += this.OnSocketAccepted;
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
            byte[] clientIv = this.ivGenerator.GetNewIv();
            byte[] serverIv = this.ivGenerator.GetNewIv();

            var session = this.sessionFactory.CreateSession();
            this.packetScheduler.Register(session);
            session.AttachSocket(e.Socket);
            this.OnConnectionOpened(session);

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