using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a game server that also handles network sessions.
    /// </summary>
    public class NetworkServer<TServerOperator> : GameServerBase
        where TServerOperator : IServerOperator
    {
        /// <summary>
        /// Gets the <see cref="IServerProcess"/> for this server.
        /// </summary>
        protected IServerProcess Process { get; }

        /// <summary>
        /// Gets the <see cref="IServerOperator"/> for this server.
        /// </summary>
        protected TServerOperator Operator { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkServer{TServerOperator}"/> type.
        /// </summary>
        /// <param name="serverProcess">The <see cref="IServerProcess"/> to use for this server.</param>
        /// <param name="serverOperator">The <see cref="IServerOperator"/> to use for this server.</param>
        protected NetworkServer(IServerProcess serverProcess, TServerOperator serverOperator)
        {
            Process = serverProcess;
            Operator = serverOperator;

            Process.ConnectionOpened += OnConnectionOpened;
        }

        private void OnConnectionOpened(object sender, ServerSessionEventArgs args)
        {
            Operator.RegisterSession(args.ServerSession);
        }

        /// <inheritdoc />
        protected override void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            Process.Configure(serviceConfiguration);
            Operator.Configure(serviceConfiguration);
        }

        /// <inheritdoc />
        protected override void OnStarting()
        {
            base.OnStarting();

            Process.Start();
        }

        /// <inheritdoc />
        protected override void OnStopping()
        {
            Process.Stop();

            base.OnStopping();
        }
    }
}
