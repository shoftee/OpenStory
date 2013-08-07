using OpenStory.Framework.Contracts;
using OpenStory.Services;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a server instance.
    /// </summary>
    public class GameServer : GameServiceBase
    {
        private readonly IServerConfigurator configurator;
        private readonly IServerProcess serverProcess;
        private readonly IServerOperator serverOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServer"/> type.
        /// </summary>
        /// <param name="configurator">The <see cref="IServerConfigurator"/> to use for this server.</param>
        /// <param name="serverProcess">The <see cref="IServerProcess"/> to use for this server.</param>
        /// <param name="serverOperator">The <see cref="IServerOperator"/> to use for this server.</param>
        public GameServer(IServerConfigurator configurator, IServerProcess serverProcess, IServerOperator serverOperator)
        {
            this.configurator = configurator;
            this.serverProcess = serverProcess;
            this.serverOperator = serverOperator;
        }

        /// <inheritdoc />
        protected override void OnConfiguring(ServiceConfiguration configuration)
        {
            this.configurator.ValidateConfiguration(configuration);

            this.serverProcess.Configure(this.ServiceConfiguration);
            this.serverOperator.Configure(this.ServiceConfiguration);
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            base.OnInitializing();

            this.serverProcess.ConnectionOpened += this.OnConnectionOpened;
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
            var session = args.ServerSession;
            this.serverOperator.RegisterSession(session);
        }
    }
}
