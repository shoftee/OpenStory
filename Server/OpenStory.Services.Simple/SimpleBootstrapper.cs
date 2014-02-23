using System.Collections.Generic;
using System.Net;
using OpenStory.Common;
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
        private readonly IKernel account;
        private readonly IKernel auth;
        private readonly IKernel world;
        private readonly IKernel channel;

        public SimpleBootstrapper(IResolutionRoot resolutionRoot, ILogger logger)
            : base(resolutionRoot, logger)
        {
            var nexus = new ChildKernel(this.ResolutionRoot, new NexusServerModule());

            this.account = new ChildKernel(nexus, new AccountServerModule());
            this.auth = CreateAuthKernel(nexus);
            this.world = new ChildKernel(nexus, new WorldServerModule());
            this.channel = new ChildKernel(this.world, new ServerModule(), new ChannelServerModule());

            // HACK :(
            nexus.Bind<IAccountService>().ToMethod(ctx => this.account.Get<IAccountService>());
        }

        private static ChildKernel CreateAuthKernel(IResolutionRoot parent)
        {
            var kernel = new ChildKernel(parent, new ServerModule(), new AuthServerModule());
            kernel.Rebind<IPacketCodeTable>().To<AuthPacketCodeTableV75>();
            return kernel;
        }

        protected override void OnStarting()
        {
            this.account.Get<IRegisteredService>().Initialize(null);

            this.Logger.Info("Preparing auth service...");
            this.auth.Get<IRegisteredService>().Initialize(this.GetAuthConfiguration());

            this.Logger.Info("Preparing world service...");
            this.world.Get<IRegisteredService>().Initialize(this.GetWorldConfiguration());

            this.Logger.Info("Preparing channel service...");
            this.channel.Get<IRegisteredService>().Initialize(this.GetChannelConfiguration());

            this.Logger.Info("Starting world service...");
            this.world.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting channel service...");
            this.channel.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting account serivce...");
            this.account.Get<IRegisteredService>().Start();

            this.Logger.Info("Starting auth service...");
            this.auth.Get<IRegisteredService>().Start();
        }

        #region Server configuration

        private OsServiceConfiguration GetAuthConfiguration()
        {
            var parameters =
                new Dictionary<string, object>
                {
                    { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8484) },
                    { "Version", (ushort)75 },
                    { "Subversion", "0" },
                    { "LocaleId", (byte)8 },
                };

            return new OsServiceConfiguration(parameters);
        }

        private OsServiceConfiguration GetWorldConfiguration()
        {
            var parameters =
                new Dictionary<string, object>
                {
                    { "World", 1 }
                };

            return new OsServiceConfiguration(parameters);
        }

        private OsServiceConfiguration GetChannelConfiguration()
        {
            var parameters =
                new Dictionary<string, object>
                {
                    { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8585) },
                    { "Version", (ushort)75 },
                    { "Subversion", "0" },
                    { "LocaleId", (byte)8 },
                };

            return new OsServiceConfiguration(parameters);
        }

        #endregion
    }
}
