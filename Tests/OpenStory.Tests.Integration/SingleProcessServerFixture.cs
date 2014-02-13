using Ninject.Extensions.Logging.Log4net;
using OpenStory.Server;
using OpenStory.Server.Accounts;
using OpenStory.Server.Auth;
using OpenStory.Server.Channel;
using OpenStory.Server.Nexus;
using OpenStory.Server.World;
using Ninject;
using Ninject.Extensions.ChildKernel;
using NUnit.Framework;

namespace OpenStory.Tests.Integration
{
    [TestFixture]
    public sealed partial class SingleProcessServerFixture
    {
        private IKernel nexus;
        private IKernel account;
        private IKernel auth;
        private IKernel world;
        private IKernel channel;

        #region Fixture stuff

        [SetUp]
        public void SetUp()
        {
            this.nexus = GetNexusKernel();
            this.account = this.GetAccountKernel();
            this.auth = this.GetAuthKernel();
            this.world = this.GetWorldKernel();
            this.channel = this.GetChannelKernel();
        }

        [TearDown]
        public void TearDown()
        {
            this.channel.Dispose();
            this.channel = null;

            this.world.Dispose();
            this.world = null;

            this.auth.Dispose();
            this.auth = null;

            this.account.Dispose();
            this.account = null;

            this.nexus.Dispose();
            this.nexus = null;
        }

        private static IKernel GetNexusKernel()
        {
            return new StandardKernel(new NexusServerModule());
        }

        private IKernel GetAccountKernel()
        {
            return new ChildKernel(this.nexus, new AccountServerModule());
        }

        private IKernel GetAuthKernel()
        {
            return new ChildKernel(this.nexus, new ServerModule(), new AuthServerModule());
        }

        private IKernel GetWorldKernel()
        {
            return new ChildKernel(this.nexus, new WorldServerModule());
        }

        private IKernel GetChannelKernel()
        {
            return new ChildKernel(this.world, new ServerModule(), new ChannelServerModule());
        }

        #endregion
    }
}
