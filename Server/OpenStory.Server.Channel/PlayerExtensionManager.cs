using System;
using System.Collections.Generic;

namespace OpenStory.Server.Channel
{
    internal sealed class PlayerExtensionManager
    {
        public static readonly PlayerExtensionManager Instance = new PlayerExtensionManager();

        private readonly Dictionary<Type, object> factories;
        private readonly Dictionary<Key, IPlayerExtension> extensions;
        
        private PlayerExtensionManager()
        {
            this.factories = new Dictionary<Type, object>();
            this.extensions = new Dictionary<Key, IPlayerExtension>();
        }

        public void Register<TPlayerExtension>(Func<TPlayerExtension> factoryMethod)
            where TPlayerExtension : IPlayerExtension
        {
            this.factories.Add(typeof(TPlayerExtension), factoryMethod);
        }

        public TPlayerExtension Create<TPlayerExtension>(int playerId)
            where TPlayerExtension : IPlayerExtension
        {
            Type extensionType = typeof(TPlayerExtension);

            var method = (Func<TPlayerExtension>)this.factories[extensionType];

            var extension = method();
            extensions.Add(new Key(extensionType, playerId), extension);
            return extension;
        }

        public TPlayerExtension Get<TPlayerExtension>(int playerId)
        {
            var extensionType = typeof(TPlayerExtension);
            var extension = extensions[new Key(extensionType, playerId)];
            return (TPlayerExtension)extension;
        }

        #region Nested type: Key

        private struct Key : IEquatable<Key>
        {
            public Type ExtensionType { get; private set; }
            public int PlayerId { get; private set; }

            public Key(Type extensionType, int playerId)
                : this()
            {
                this.ExtensionType = extensionType;
                this.PlayerId = playerId;
            }

            #region Implementation of IEquatable<Key>

            public bool Equals(Key other)
            {
                return this.ExtensionType == other.ExtensionType
                    && this.PlayerId == other.PlayerId;
            }

            #endregion

            public override bool Equals(object obj)
            {
                return obj is Key && Equals((Key)obj);
            }

            #region Equality members

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((this.ExtensionType != null ? this.ExtensionType.GetHashCode() : 0) * 397) ^ this.PlayerId;
                }
            }

            public static bool operator ==(Key left, Key right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Key left, Key right)
            {
                return !left.Equals(right);
            }

            #endregion
        }

        #endregion
    }

}
