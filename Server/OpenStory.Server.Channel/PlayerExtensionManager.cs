using System;
using System.Collections.Generic;

namespace OpenStory.Server.Channel
{
    class PlayerExtensionManager
    {
        private readonly Dictionary<Type, object> factories;

        private readonly Dictionary<PlayerExtensionKey, IPlayerExtension> extensions;

        private PlayerExtensionManager()
        {
            this.factories = new Dictionary<Type, object>();
            this.extensions = new Dictionary<PlayerExtensionKey, IPlayerExtension>();
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
            extensions.Add(new PlayerExtensionKey(extensionType, playerId), extension);
            return extension;
        }

        public TPlayerExtension Get<TPlayerExtension>(int playerId)
        {
            var extensionType = typeof(TPlayerExtension);
            var extension = extensions[new PlayerExtensionKey(extensionType, playerId)];
            return (TPlayerExtension)extension;
        }
    }

    struct PlayerExtensionKey : IEquatable<PlayerExtensionKey>
    {
        public Type ExtensionType { get; private set; }
        public int PlayerId { get; private set; }

        public PlayerExtensionKey(Type extensionType, int playerId)
            : this()
        {
            this.ExtensionType = extensionType;
            this.PlayerId = playerId;
        }

        #region Implementation of IEquatable<PlayerExtensionKey>

        public bool Equals(PlayerExtensionKey other)
        {
            return this.ExtensionType == other.ExtensionType
                && this.PlayerId == other.PlayerId;
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is PlayerExtensionKey && Equals((PlayerExtensionKey)obj);
        }

        #region Equality members

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.ExtensionType != null ? this.ExtensionType.GetHashCode() : 0) * 397) ^ this.PlayerId;
            }
        }

        public static bool operator ==(PlayerExtensionKey left, PlayerExtensionKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlayerExtensionKey left, PlayerExtensionKey right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
