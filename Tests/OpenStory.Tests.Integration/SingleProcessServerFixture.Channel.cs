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
        private static readonly Type[] ChannelResolutions =
        {
            typeof(IServerOperator),
            typeof(IServerProcess),
            typeof(IServiceContainer<IWorldToChannelRequestHandler>),
        };

        [Test]
        public void Channel_Should_Resolve([ValueSource("ChannelResolutions")] Type type)
        {
            this.channel.TryGet(type).Should().NotBeNull();
        }
    }
}
