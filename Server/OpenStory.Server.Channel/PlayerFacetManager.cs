using System;
using System.Collections.Generic;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    internal sealed class PlayerFacetManager
    {
        private readonly IPlayerFacetFactory _factory;

        private readonly Dictionary<Key, IPlayerFacet> _facets;

        public PlayerFacetManager(IPlayerFacetFactory playerFacetFactory)
        {
            _factory = playerFacetFactory;

            _facets = new Dictionary<Key, IPlayerFacet>();
        }

        public TPlayerFacet Create<TPlayerFacet>(CharacterKey characterKey)
            where TPlayerFacet : IPlayerFacet
        {
            var facet = _factory.CreateFacet<TPlayerFacet>(characterKey);

            var facetKey = new Key(typeof(TPlayerFacet), characterKey.Id);
            _facets.Add(facetKey, facet);

            return facet;
        }

        public TPlayerFacet Get<TPlayerFacet>(CharacterKey key)
        {
            var facetKey = new Key(typeof(TPlayerFacet), key.Id);
            var facet = _facets[facetKey];
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
                FacetType = facetType;
                PlayerId = playerId;
            }

            #region Implementation of IEquatable<Key>

            public bool Equals(Key other)
            {
                return FacetType == other.FacetType
                    && PlayerId == other.PlayerId;
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
                    return ((FacetType != null ? FacetType.GetHashCode() : 0) * 397) ^ PlayerId;
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
