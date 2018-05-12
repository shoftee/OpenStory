using System.Collections.Generic;
using System.Net;
using OpenStory.Common;
using OpenStory.Framework.Contracts;
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
        private readonly IKernel _account;
        private readonly IKernel _auth;
        private readonly IKernel _world;
        private readonly IKernel _channel;

        public SimpleBootstrapper(IResolutionRoot resolutionRoot, ILogger logger)
            : base(resolutionRoot, logger)
        {
            var nexus = new ChildKernel(ResolutionRoot, new NexusServerModule());

            _account = new ChildKernel(nexus, new AccountServerModule());
            _auth = CreateAuthKernel(nexus);
            _world = new ChildKernel(nexus, new WorldServerModule());
            _channel = new ChildKernel(_world, new ServerModule(), new ChannelServerModule());

            // HACK :(
            nexus.Bind<IAccountService>().ToMethod(ctx => _account.Get<IAccountService>());
        }

        private static ChildKernel CreateAuthKernel(IResolutionRoot parent)
        {
            var kernel = new ChildKernel(parent, new ServerModule(), new AuthServerModule());
            kernel.Rebind<IPacketCodeTable>().To<AuthPacketCodeTableV75>().InSingletonScope();
            kernel.Rebind<IAuthenticator>().To<StubAuthenticator>().InSingletonScope();
            kernel.Rebind<IAccountProvider>().To<StubAccountProvider>().InSingletonScope();
            return kernel;
        }

        protected override void OnStarting()
        {
            _account.Get<IRegisteredService>().Initialize(null);

            Logger.Info("Preparing auth service...");
            _auth.Get<IRegisteredService>().Initialize(GetAuthConfiguration());

            Logger.Info("Preparing world service...");
            _world.Get<IRegisteredService>().Initialize(GetWorldConfiguration());

            Logger.Info("Preparing channel service...");
            _channel.Get<IRegisteredService>().Initialize(GetChannelConfiguration());

            Logger.Info("Starting world service...");
            _world.Get<IRegisteredService>().Start();

            Logger.Info("Starting channel service...");
            _channel.Get<IRegisteredService>().Start();

            Logger.Info("Starting account service...");
            _account.Get<IRegisteredService>().Start();

            Logger.Info("Starting auth service...");
            _auth.Get<IRegisteredService>().Start();
        }

        #region Server configuration

        private OsServiceConfiguration GetAuthConfiguration()
        {
            var parameters =
                new Dictionary<string, object>
                {
                    { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8484) },
                    { "Version", (ushort)75 },
                    { "Subversion", "" },
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
                    { "Subversion", "" },
                    { "LocaleId", (byte)8 },
                };

            return new OsServiceConfiguration(parameters);
        }

        #endregion
    }
}
