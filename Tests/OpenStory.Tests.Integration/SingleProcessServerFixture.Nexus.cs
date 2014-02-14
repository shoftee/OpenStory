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
        private static readonly Type[] NexusResolutions =
        {
            typeof(IServerOperator),
            typeof(IServiceContainer<INexusToWorldRequestHandler>),
            typeof(IAuthToNexusRequestHandler),
        };

        [Test]
        public void Nexus_Should_Resolve([ValueSource("NexusResolutions")] Type type)
        {
            this.auth.TryGet(type).Should().NotBeNull();
        }
    }
}
