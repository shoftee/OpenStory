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
        [TestCase(typeof(IServiceContainer<INexusToWorldRequestHandler>))]
        [TestCase(typeof(IAuthToNexusRequestHandler))]
        public void Nexus_Should_Resolve(Type type)
        {
            _auth.TryGet(type).Should().NotBeNull();
        }
    }
}
