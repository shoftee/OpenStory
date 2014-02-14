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
        private static readonly Type[] AuthResolutions =
        {
            typeof(IServerOperator),
            typeof(IServerProcess),
        };

        [Test]
        public void Auth_Should_Resolve([ValueSource("AuthResolutions")] Type type)
        {
            this.auth.TryGet(type).Should().NotBeNull();
        }
    }
}
