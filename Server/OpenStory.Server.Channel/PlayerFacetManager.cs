using System;
using System.Collections.Generic;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    internal sealed class PlayerFacetManager
    {
        private readonly IPlayerFacetFactory factory;

        private readonly Dictionary<Key, IPlayerFacet> facets;

        public PlayerFacetManager(IPlayerFacetFactory playerFacetFactory)
        {
            this.factory = playerFacetFactory;

            this.facets = new Dictionary<Key, IPlayerFacet>();
        }

        public TPlayerFacet Create<TPlayerFacet>(CharacterKey characterKey)
            where TPlayerFacet : IPlayerFacet
        {
            var facet = this.factory.CreateFacet<TPlayerFacet>(characterKey);

            var facetKey = new Key(typeof(TPlayerFacet), characterKey.Id);
            this.facets.Add(facetKey, facet);

            return facet;
        }

        public TPlayerFacet Get<TPlayerFacet>(CharacterKey key)
        {
            var facetKey = new Key(typeof(TPlayerFacet), key.Id);
            var facet = this.facets[facetKey];
            return (TPlayerFacet)facet;
        }

        #region Nested type: Key

        private struct Key : IEquatable<Key>
        {
            public Type FacetType { get; }
            public int PlayerId { get; }

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
                return obj is Key && this.Equals((Key)obj);
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
