using System;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;
using OpenStory.Tests.Helpers;

namespace OpenStory.Common.Game
{
    [Category("OpenStory.Common.Game.KeyLayout")]
    [TestFixture]
    public sealed class KeyLayoutFixture
    {
        private static KeyBinding[] DummyBindingList
        {
            get
            {
                return Enumerable.Range(0, GameConstants.KeyCount)
                                 .Select(i => new KeyBinding((byte)i, i))
                                 .ToArray();
            }
        }

        private static KeyBinding[] DummyBindingListIncorrect
        {
            get
            {
                return Enumerable.Range(0, GameConstants.KeyCount + 1)
                                 .Select(i => new KeyBinding((byte)i, i))
                                 .ToArray();
            }
        }

        private static byte InvalidKeyId
        {
            get { return (byte)(GameConstants.KeyCount + 1); }
        }

        [Test]
        public void Constructor_Should_Not_Throw_On_Correct_Binding_Count()
        {
            Action construction = () => new KeyLayout(DummyBindingList);
            construction.ShouldNotThrow();
        }

        [Test]
        public void Constructor_Should_Throw_On_Incorrect_Binding_Count()
        {
            Action construction = () => new KeyLayout(DummyBindingListIncorrect);
            construction
                .ShouldThrow<ArgumentException>()
                .WithMessageFormat(CommonStrings.WrongKeyBindingCount, GameConstants.KeyCount);
        }

        [Test]
        public void Constructor_Should_Throw_On_Null_Collection()
        {
            Action construction = () => new KeyLayout(null);
            construction
                .ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Constructed_Bindings_List_Should_Be_The_Same()
        {
            var layout = new KeyLayout(DummyBindingList);

            layout.Bindings.Should().HaveSameCount(DummyBindingList);
            layout.Bindings.ShouldAllBeEquivalentTo(
                DummyBindingList,
                eq => eq.Including(kb => kb.ActionId)
                        .Including(kb => kb.ActionTypeId));
        }

        [Test]
        public void GetKeyBinding_Should_Throw_On_Invalid_Id()
        {
            var layout = new KeyLayout(DummyBindingList);

            layout
                .Invoking(l => l.GetKeyBinding(InvalidKeyId))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetKeyBinding_Should_Return_Correct_Binding()
        {
            var layout = new KeyLayout(DummyBindingList);
            VerifyKeyBinding(layout.GetKeyBinding(0), layout.Bindings[0]);
        }

        private static void VerifyKeyBinding(KeyBinding actual, KeyBinding expected)
        {
            actual.ShouldBeEquivalentTo(expected, SetUpKeyBindingEquivalency);
        }

        private static EquivalencyAssertionOptions<KeyBinding> SetUpKeyBindingEquivalency(EquivalencyAssertionOptions<KeyBinding> options)
        {
            return
                options
                    .Including(b => b.ActionTypeId)
                    .Including(b => b.ActionId);
        }

        [Test]
        public void SetKeyBinding_Should_Throw_On_Invalid_Id()
        {
            var layout = new KeyLayout(DummyBindingList);
            layout
                .Invoking(l => l.SetKeyBinding(InvalidKeyId, 0, 0))
                .ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void SetKeyBinding_Should_Set_Correct_Binding()
        {
            var layout = new KeyLayout(DummyBindingList.ToArray());
            layout.SetKeyBinding(0, 1, 10);

            VerifyKeyBinding(layout.GetKeyBinding(0), layout.Bindings[0]);
        }
    }
}
