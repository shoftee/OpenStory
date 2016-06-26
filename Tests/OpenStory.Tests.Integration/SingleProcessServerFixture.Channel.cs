using System;
using FluentAssertions;
using Ninject;
using NUnit.Framework;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Tests.Integration
{
    public sealed partial class SingleProcessServerFixture
    {
        [TestCase(typeof(IServerOperator))]
        [TestCase(typeof(IServerProcess))]
        [TestCase(typeof(IServiceContainer<IWorldToChannelRequestHandler>))]
        public void Channel_Should_Resolve(Type type)
        {
            this.channel.TryGet(type).Should().NotBeNull();
        }
    }
}
