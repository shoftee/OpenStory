using OpenStory.Framework.Contracts;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a server instance.
    /// </summary>
    public class GameServer : RegisteredServiceBase
    {
        private readonly IServerProcess serverProcess;
        private readonly IServerOperator serverOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServer"/> type.
        /// </summary>
        /// <param name="serverProcess">The <see cref="IServerProcess"/> to use for this server.</param>
        /// <param name="serverOperator">The <see cref="IServerOperator"/> to use for this server.</param>
        public GameServer(IServerProcess serverProcess, IServerOperator serverOperator)
        {
            this.serverProcess = serverProcess;
            this.serverOperator = serverOperator;

            this.serverProcess.ConnectionOpened += this.OnConnectionOpened;
        }

        /// <inheritdoc />
        protected override void OnInitializing(ServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            this.serverProcess.Configure(serviceConfiguration);
            this.serverOperator.Configure(serviceConfiguration);
        }

        /// <inheritdoc />
        protected override void OnStarting()
        {
            base.OnStarting();

            this.serverProcess.Start();
        }

        /// <inheritdoc />
        protected override void OnStopping()
        {
            this.serverProcess.Stop();

            base.OnStopping();
        }

        private void OnConnectionOpened(object sender, ServerSessionEventArgs args)
        {
            this.serverOperator.RegisterSession(args.ServerSession);
        }
    }
}
