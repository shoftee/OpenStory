using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Networking;
using OpenStory.Services.Contracts;
using FluentAssertions;
using Moq;
using Ninject.Extensions.Logging;
using NUnit.Framework;

namespace OpenStory.Server.Processing
{
    [TestFixture]
    public sealed class ServerProcessFixture
    {
        private IServerSessionFactory serverSessionFactory;
        private ISocketAcceptorFactory socketAcceptorFactory;
        private IPacketScheduler packetScheduler;
        private IRollingIvFactoryProvider rollingIvFactoryProvider;
        private IvGenerator ivGenerator;
        private ILogger logger;

        [SetUp]
        public void SetUp()
        {
            this.serverSessionFactory = Mock.Of<IServerSessionFactory>();

            var socketAcceptorFactoryMock = new Mock<ISocketAcceptorFactory>();
            socketAcceptorFactoryMock
                .Setup(saf => saf.CreateSocketAcceptor(It.IsAny<IPEndPoint>()))
                .Returns<IPEndPoint>(endpoint => new SocketAcceptor(endpoint));
            this.socketAcceptorFactory = socketAcceptorFactoryMock.Object;
            
            this.packetScheduler = Mock.Of<IPacketScheduler>();
            this.rollingIvFactoryProvider = Mock.Of<IRollingIvFactoryProvider>();
            this.ivGenerator = new IvGenerator(new RNGCryptoServiceProvider());
            this.logger = Mock.Of<ILogger>();
        }

        [TearDown]
        public void TearDown()
        {
            this.serverSessionFactory = null;
            this.socketAcceptorFactory = null;
            this.packetScheduler = null;
            this.rollingIvFactoryProvider = null;
            this.ivGenerator = null;
            this.logger = null;
        }

        [Test]
        public void Configure_Should_Create_Acceptor_And_RollingIvFactory()
        {
            var process = this.CreateServerProcess();

            process.Configure(this.CreateOsServiceConfiguration());

            Mock.Get(this.socketAcceptorFactory).Verify(ThatSocketAcceptorIsCreatedCorrectly(), Times.Once);
            Mock.Get(this.rollingIvFactoryProvider).Verify(ThatRollingIvFactoryIsCreatedCorrectly(), Times.Once);
        }

        [Test]
        public void Start_Should_Throw_When_Not_Configured()
        {
            var process = this.CreateServerProcess();

            process
                .Invoking(p => p.Start())
                .ShouldThrow<InvalidOperationException>("because the server must be configured before it can run");
        }

        [Test]
        public void Start_Should_Throw_When_Already_Running()
        {
            var process = this.CreateServerProcess();

            var osServiceConfiguration = this.CreateOsServiceConfiguration();
            process.Configure(osServiceConfiguration);

            process
                .Invoking(p => p.Start())
                .ShouldNotThrow();
            
            process
                .Invoking(p => p.Start())
                .ShouldThrow<InvalidOperationException>("because the server should already be running")
                .WithMessage(ServerStrings.ServerAlreadyRunning);
        }

        [Test]
        public void Configure_Should_Throw_When_Already_Running()
        {
            var process = this.CreateServerProcess();
            
            var osServiceConfiguration = this.CreateOsServiceConfiguration();
            process
                .Invoking(p => p.Configure(osServiceConfiguration))
                .ShouldNotThrow();

            process
                .Start();

            process
                .Invoking(p => p.Configure(It.IsAny<OsServiceConfiguration>()))
                .ShouldThrow<InvalidOperationException>("because the server cannot be configured once it is started")
                .WithMessage(ServerStrings.ServerAlreadyRunning);
        }

        private static Expression<Func<ISocketAcceptorFactory, SocketAcceptor>> ThatSocketAcceptorIsCreatedCorrectly()
        {
            return saf => saf.CreateSocketAcceptor(It.Is<IPEndPoint>(v => IPAddress.IsLoopback(v.Address) && v.Port == 0));
        }

        private static Expression<Func<IRollingIvFactoryProvider, RollingIvFactory>> ThatRollingIvFactoryIsCreatedCorrectly()
        {
            return rifp => rifp.CreateFactory(It.Is<ushort>(v => v == 84));
        }

        private IServerProcess CreateServerProcess()
        {
            var process = new ServerProcess(
                this.serverSessionFactory, 
                this.socketAcceptorFactory, 
                this.packetScheduler,
                this.rollingIvFactoryProvider, 
                this.ivGenerator, 
                this.logger);
            return process;
        }

        private OsServiceConfiguration CreateOsServiceConfiguration()
        {
            var parameters =
                new Dictionary<string, object>
                {
                    { "Endpoint", new IPEndPoint(IPAddress.Loopback, 0) },
                    { "Header", (ushort)14 },
                    { "Version", (ushort)84 },
                    { "Subversion", "" },
                    { "LocaleId", (byte) 9 }
                };

            return new OsServiceConfiguration(parameters);

        }
    }
}
