namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Phase of the login process.
    /// </summary>
    public enum AuthClientState
    {
        /// <summary>
        /// Default value, not to be used.
        /// </summary>
        None = 0,

        /// <summary>
        /// The client has yet to authenticate.
        /// </summary>
        PreAuthentication,

        /// <summary>
        /// The client has authenticated, but hasn't reached World Select yet.
        /// </summary>
        PostAuthentication,

        /// <summary>
        /// The client is in the world selection screen.
        /// </summary>
        WorldSelect,

        /// <summary>
        /// The client is in the channel selection screen.
        /// </summary>
        ChannelSelect,

        /// <summary>
        /// The client is at the character selection screen.
        /// </summary>
        CharacterSelect,

        /// <summary>
        /// The client has selected a character, but not yet in server transition.
        /// </summary>
        PostCharacterSelect,

        /// <summary>
        /// The client is in the middle of the server transition.
        /// </summary>
        Transition
    }
}
