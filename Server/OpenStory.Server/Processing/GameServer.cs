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
        private readonly IServerFactory serverFactory;

        private IServerProcess serverProcess;
        private IServerOperator serverOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServer"/> type.
        /// </summary>
        /// <param name="configurator">The <see cref="IServerConfigurator"/> to use for this server.</param>
        /// <param name="serverFactory">The <see cref="IServerFactory"/> to use when creating <see cref="IServerOperator"/> objects.</param>
        public GameServer(
            IServerConfigurator configurator,
            IServerFactory serverFactory)
        {
            this.configurator = configurator;
            this.serverFactory = serverFactory;
        }

        /// <inheritdoc />
        protected override void OnConfiguring(ServiceConfiguration configuration)
        {
            this.configurator.ValidateConfiguration(configuration);
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            base.OnInitializing();
            

            this.serverProcess = this.serverFactory.CreateProcess();
            this.serverProcess.Configure(this.ServiceConfiguration);

            this.serverOperator = this.serverFactory.CreateOperator();
            this.serverOperator.Configure(this.ServiceConfiguration);

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
