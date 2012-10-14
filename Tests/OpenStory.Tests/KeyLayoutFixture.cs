using System;
using System.Linq;
using NUnit.Framework;
using OpenStory.Common.Game;

namespace OpenStory.Tests
{
    [TestFixture(Category = "OpenStory.Common.Game", Description = "KeyBinding tests.")]
    public sealed class KeyLayoutFixture
    {
        [Test]
        public void DoesNotThrowOnCorrectNumberOfBindings()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            Assert.DoesNotThrow(() => new KeyLayout(bindings));
        }

        [Test]
        public void ThrowsOnIncorrectNumberOfBindings()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount + 1).Select(i => new KeyBinding((byte)i, i)).ToArray();
            Assert.Throws<ArgumentException>(() => new KeyLayout(bindings));
        }

        [Test]
        public void ThrowsOnNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => new KeyLayout(null));
        }

        [Test]
        public void BindingListIsTheSame()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            var layout = new KeyLayout(bindings);
            CollectionAssert.AreEqual(bindings, layout.Bindings);
        }

        [Test]
        public void GetKeyBindingsThrowsOnInvalidId()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            var layout = new KeyLayout(bindings);

            Assert.Throws<ArgumentOutOfRangeException>(() => layout.GetKeyBinding((byte)(GameConstants.KeyCount + 1)));
        }

        [Test]
        public void GetKeyBindingsWorksCorrectly()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            var layout = new KeyLayout(bindings);

            var binding = layout.GetKeyBinding(0);

            Assert.AreEqual(binding.ActionTypeId, layout.Bindings[0].ActionTypeId);
            Assert.AreEqual(binding.ActionId, layout.Bindings[0].ActionId);
        }

        [Test]
        public void SetKeyBindingsThrowsOnInvalidId()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            var layout = new KeyLayout(bindings);

            Assert.Throws<ArgumentOutOfRangeException>(() => layout.SetKeyBinding((byte)(GameConstants.KeyCount + 1), 0, 0));
        }

        [Test]
        public void SetKeyBindingsWorksCorrectly()
        {
            var bindings = Enumerable.Range(0, GameConstants.KeyCount).Select(i => new KeyBinding((byte)i, i)).ToArray();
            var layout = new KeyLayout(bindings);

            layout.SetKeyBinding(0, 1, 10);
            var binding = layout.GetKeyBinding(0);

            Assert.AreEqual(binding.ActionTypeId, layout.Bindings[0].ActionTypeId);
            Assert.AreEqual(binding.ActionId, layout.Bindings[0].ActionId);
        }
    }
}
