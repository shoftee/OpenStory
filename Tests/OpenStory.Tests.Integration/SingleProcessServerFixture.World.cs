using System;
using FluentAssertions;
using Ninject;
using NUnit.Framework;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Tests.Integration
{
    public sealed partial class SingleProcessServerFixture
    {
        private static readonly Type[] WorldResolutions =
        {
            typeof(IServiceContainer<INexusToWorldRequestHandler>),
            typeof(IWorldInfoProvider),
        };

        [Test]
        public void World_Should_Resolve([ValueSource("WorldResolutions")] Type type)
        {
            this.world.TryGet(type).Should().NotBeNull();
        }
    }
}
