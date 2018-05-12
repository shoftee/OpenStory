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
        [TestCase(typeof(IServiceContainer<INexusToWorldRequestHandler>))]
        [TestCase(typeof(IWorldInfoProvider))]
        public void World_Should_Resolve(Type type)
        {
            _world.TryGet(type).Should().NotBeNull();
        }
    }
}
