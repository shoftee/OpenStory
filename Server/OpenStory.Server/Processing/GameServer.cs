using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a server instance.
    /// </summary>
    public class GameServer : RegisteredServiceBase
    {
        private readonly IServerProcess serverProcess;
        private readonly IServerOperator channelOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameServer"/> type.
        /// </summary>
        /// <param name="serverProcess">The <see cref="IServerProcess"/> to use for this server.</param>
        /// <param name="channelOperator">The <see cref="IServerOperator"/> to use for this server.</param>
        protected GameServer(IServerProcess serverProcess, IServerOperator channelOperator)
        {
            this.serverProcess = serverProcess;
            this.channelOperator = channelOperator;

            this.serverProcess.ConnectionOpened += this.OnConnectionOpened;
        }

        private void OnConnectionOpened(object sender, ServerSessionEventArgs args)
        {
            this.channelOperator.RegisterSession(args.ServerSession);
        }

        /// <inheritdoc />
        protected override void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            this.serverProcess.Configure(serviceConfiguration);
            this.channelOperator.Configure(serviceConfiguration);
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
    }
}
