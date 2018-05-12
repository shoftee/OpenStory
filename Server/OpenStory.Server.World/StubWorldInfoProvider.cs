using System.Collections.Generic;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.World
{
    /// <summary>
    /// Provides... world info...
    /// </summary>
    internal sealed class StubWorldInfoProvider : IWorldInfoProvider
    {
        private readonly Dictionary<int, WorldInfo> _worlds =
            new Dictionary<int, WorldInfo>()
            {
                { 1, new WorldInfo() { WorldId = 1, WorldName = "Tespia", ChannelCount = 1, } }
            };

        /// <inheritdoc/>
        public WorldInfo GetWorldById(int id)
        {
            WorldInfo worldInfo;
            _worlds.TryGetValue(id, out worldInfo);
            return worldInfo;
        }

        /// <inheritdoc/>
        public IEnumerable<WorldInfo> GetAllWorlds()
        {
            return _worlds.Values;
        }
    }
}