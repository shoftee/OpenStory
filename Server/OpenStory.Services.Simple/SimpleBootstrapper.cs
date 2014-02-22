using OpenStory.Server;
using OpenStory.Server.Accounts;
using OpenStory.Server.Auth;
using OpenStory.Server.Channel;
using OpenStory.Server.Nexus;
using OpenStory.Server.World;
using OpenStory.Services.Contracts;
using Ninject;
using Ninject.Extensions.ChildKernel;
using Ninject.Extensions.Logging;
using Ninject.Syntax;

namespace OpenStory.Services.Simple
{
    public class SimpleBootstrapper : BootstrapperBase
    {
        private readonly IKernel nexus;
        private readonly IKernel account;
        private readonly IKernel auth;
        private readonly IKernel world;
        private readonly IKernel channel;

        public SimpleBootstrapper(IResolutionRoot resolutionRoot, ILogger logger)
            : base(resolutionRoot, logger)
        {
            this.nexus = new ChildKernel(this.ResolutionRoot, new NexusServerModule());
            this.account = new ChildKernel(this.nexus, new AccountServerModule());
            this.auth = new ChildKernel(this.nexus, new ServerModule(), new AuthServerModule());
            this.world = new ChildKernel(this.nexus, new WorldServerModule());
            this.channel = new ChildKernel(this.world, new ServerModule(), new ChannelServerModule());
        }

        protected override void OnStarting()
        {
            this.Logger.Info("Preparing account service...");
            this.account.Get<IRegisteredService>().Initialize(null);

            this.Logger.Info("Preparing auth service...");
            this.auth.Get<IRegisteredService>().Initialize(null);

            this.Logger.Info("Preparing world service...");
            this.world.Get<IRegisteredService>().Initialize(null);

            this.Logger.Info("Preparing channel service...");
            this.channel.Get<IRegisteredService>().Initialize(null);

            this.Logger.Info("Starting world service...");
            this.world.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting channel service...");
            this.channel.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting account serivce...");
            this.account.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting auth service...");
            this.auth.Get<IRegisteredService>().Start();
        }
    }
}
