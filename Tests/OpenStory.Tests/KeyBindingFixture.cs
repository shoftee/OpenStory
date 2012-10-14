using NUnit.Framework;
using OpenStory.Common.Game;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.Game", Description = "KeyBinding tests.")]
    public sealed class KeyBindingFixture
    {
        [Test]
        public void DoesNotThrowOnCreation()
        {
            Assert.DoesNotThrow(() => new KeyBinding(0, 0));
        }
    }
}