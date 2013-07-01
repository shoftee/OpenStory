using System;
using System.Collections.Generic;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    internal sealed class PlayerFacetManager
    {
        public static readonly PlayerFacetManager Instance = new PlayerFacetManager();

        private readonly Dictionary<Type, object> factories;
        private readonly Dictionary<Key, IPlayerFacet> facets;
        
        private PlayerFacetManager()
        {
            this.factories = new Dictionary<Type, object>();
            this.facets = new Dictionary<Key, IPlayerFacet>();
        }

        public void Register<TPlayerFacet>(Func<TPlayerFacet> factoryMethod)
            where TPlayerFacet : IPlayerFacet
        {
            this.factories.Add(typeof(TPlayerFacet), factoryMethod);
        }

        public TPlayerFacet Create<TPlayerFacet>(CharacterKey key)
            where TPlayerFacet : IPlayerFacet
        {
            var facetType = typeof(TPlayerFacet);
            var method = (Func<TPlayerFacet>)this.factories[facetType];

            var facet = method();
            this.facets.Add(new Key(facetType, key.Id), facet);
            return facet;
        }

        public TPlayerFacet Get<TPlayerFacet>(CharacterKey key)
        {
            var facetType = typeof(TPlayerFacet);
            var facet = this.facets[new Key(facetType, key.Id)];
            return (TPlayerFacet)facet;
        }

        #region Nested type: Key

        private struct Key : IEquatable<Key>
        {
            public Type FacetType { get; private set; }
            public int PlayerId { get; private set; }

            public Key(Type facetType, int playerId)
                : this()
            {
                this.FacetType = facetType;
                this.PlayerId = playerId;
            }

            #region Implementation of IEquatable<Key>

            public bool Equals(Key other)
            {
                return this.FacetType == other.FacetType
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
                    return ((this.FacetType != null ? this.FacetType.GetHashCode() : 0) * 397) ^ this.PlayerId;
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
