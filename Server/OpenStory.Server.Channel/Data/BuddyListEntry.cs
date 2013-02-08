using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Channel.Data
{
    /// <summary>
    /// Represents an entry in a player's buddy list.
    /// </summary>
    public class BuddyListEntry : ICharacterExtension
    {
        /// <summary>
        /// Gets the identifier of the buddy.
        /// </summary>
        public CharacterKey Key { get; private set; }

        /// <summary>
        /// Gets the group name of the buddy.
        /// </summary>
        public string Group { get; private set; }

        /// <summary>
        /// Gets the channel identifier for the buddy.
        /// </summary>
        public int? ChannelId { get; private set; }

        /// <summary>
        /// Gets whether the buddy is currently visible.
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// Gets the status of the buddy.
        /// </summary>
        public BuddyListEntryStatus Status { get; private set; }
    }
}
