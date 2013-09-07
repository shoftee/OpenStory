using System;
using FluentAssertions;
using NUnit.Framework;

namespace OpenStory.Common.Game
{
    [TestFixture]
    [Category("OpenStory.Common.Game.KeyBinding")]
    public sealed class KeyBindingFixture
    {
        [Test]
        public void Constructor_Should_Not_Throw()
        {
            Action construction = () => new KeyBinding(0, 0);
            construction.ShouldNotThrow();
        }
    }
}