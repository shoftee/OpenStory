using FluentAssertions;
using Ninject;
using Ninject.Extensions.ChildKernel;
using NUnit.Framework;
using OpenStory.Server;
using OpenStory.Server.Accounts;
using OpenStory.Server.Auth;
using OpenStory.Server.Channel;
using OpenStory.Server.Processing;
using OpenStory.Server.World;

namespace OpenStory.Tests.Integration
{
    [TestFixture]
    public sealed class SingleProcessServerFixture
    {
        private IKernel common;
        private IKernel account;
        private IKernel auth;
        private IKernel world;
        private IKernel channel;

        [SetUp]
        public void SetUp()
        {
            this.common = new StandardKernel(new ServerModule());
            this.account = new ChildKernel(this.common, new AccountServerModule());
            this.auth = new ChildKernel(this.common, new AuthServerModule());
            this.world = new ChildKernel(this.common, new WorldServerModule());
            this.channel = new ChildKernel(this.world, new ChannelServerModule());
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

            this.common.Dispose();
            this.common = null;
        }

        [Test]
        public void Auth_Kernel_Should_Resolve_Auth_Operator()
        {
            this.auth.Get<IServerOperator>().Should().BeAssignableTo<AuthOperator>();
        }

        [Test]
        public void Channel_Kernel_Should_Resolve_Channel_Operator()
        {
            this.channel.Get<IServerOperator>().Should().BeAssignableTo<ChannelOperator>();
        }
    }
}
