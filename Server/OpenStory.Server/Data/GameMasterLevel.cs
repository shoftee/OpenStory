using System;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Permission levels for Game Master users.
    /// </summary>
    [Serializable]
    public enum GameMasterLevel : byte
    {
        /// <summary>
        /// The user is a normal player and has no game master priviledges.
        /// </summary>
        None = 0,

        /// <summary>
        /// The use has some game master abilities.
        /// </summary>
        GameMasterHelper = 1,

        /// <summary>
        /// The user is a Game Master.
        /// </summary>
        GameMaster = 2
    }
}
