using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Represents a registry for player locations.
    /// </summary>
    internal sealed class LocationRegistry : ILocationRegistry
    {
        private readonly Dictionary<CharacterKey, PlayerLocation> _locations;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationRegistry"/> class.
        /// </summary>
        public LocationRegistry()
        {
            _locations = new Dictionary<CharacterKey, PlayerLocation>();
        }

        /// <inheritdoc />
        public Dictionary<CharacterKey, PlayerLocation> GetLocationsForAll(IEnumerable<CharacterKey> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            return keys
                .Distinct()
                .ToDictionary(key => key, GetLocation);
        }

        /// <inheritdoc />
        public PlayerLocation GetLocation(CharacterKey key)
        {
            PlayerLocation location;
            _locations.TryGetValue(key, out location);
            return location;
        }

        /// <inheritdoc />
        public void SetLocation(CharacterKey key, int channelId, int mapId)
        {
            var location = new PlayerLocation(channelId, mapId);
            if (_locations.ContainsKey(key))
            {
                _locations[key] = location;
            }
            else
            {
                _locations.Add(key, location);
            }
        }

        /// <inheritdoc />
        public void RemoveLocation(CharacterKey key)
        {
            _locations.Remove(key);
        }
    }
}
