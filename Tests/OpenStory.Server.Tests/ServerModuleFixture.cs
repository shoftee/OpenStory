using System;
using System.Security.Cryptography;
using FluentAssertions;
using NUnit.Framework;
using Ninject;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;
using OpenStory.Services;

namespace OpenStory.Server.Tests
{
    [Category("OpenStory.Server.Integration")]
    [TestFixture]
    public sealed class ServerModuleFixture
    {
        [Test]
        public void Should_Have_Type_Binding(
            [Values(
                typeof(IPlayerRegistry),
                typeof(ILocationRegistry),
                typeof(IPacketScheduler),
                typeof(IServiceConfigurationProvider),
                typeof(INexusConnectionProvider),
                typeof(IPacketFactory),
                typeof(IServerSession),
                typeof(IServerSessionFactory),
                typeof(RandomNumberGenerator),
                typeof(IvGenerator),
                typeof(IServerProcess),
                typeof(GameServiceBase),
                typeof(Bootstrapper)
                )] Type type)
        {
            var kernel = GetServerKernel();
            kernel.GetBindings(type).Should().HaveCount(1);
        }

        private static StandardKernel GetServerKernel()
        {
            return new StandardKernel(new ServerModule());
        } 
    }
}
