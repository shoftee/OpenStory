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
        private IKernel _nexus;
        private IKernel _account;
        private IKernel _auth;
        private IKernel _world;
        private IKernel _channel;

        #region Fixture stuff

        [SetUp]
        public void SetUp()
        {
            _nexus = GetNexusKernel();
            _account = GetAccountKernel();
            _auth = GetAuthKernel();
            _world = GetWorldKernel();
            _channel = GetChannelKernel();
        }

        [TearDown]
        public void TearDown()
        {
            _channel.Dispose();
            _channel = null;

            _world.Dispose();
            _world = null;

            _auth.Dispose();
            _auth = null;

            _account.Dispose();
            _account = null;

            _nexus.Dispose();
            _nexus = null;
        }

        private static IKernel GetNexusKernel()
        {
            return new StandardKernel(new NexusServerModule());
        }

        private IKernel GetAccountKernel()
        {
            return new ChildKernel(_nexus, new AccountServerModule());
        }

        private IKernel GetAuthKernel()
        {
            return new ChildKernel(_nexus, new ServerModule(), new AuthServerModule());
        }

        private IKernel GetWorldKernel()
        {
            return new ChildKernel(_nexus, new WorldServerModule());
        }

        private IKernel GetChannelKernel()
        {
            return new ChildKernel(_world, new ServerModule(), new ChannelServerModule());
        }

        #endregion
    }
}
