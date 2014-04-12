namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Phase of the login process.
    /// </summary>
    public enum AuthClientState
    {
        /// <summary>
        /// The client has yet to authenticate. Default value.
        /// </summary>
        NotLoggedIn = 0,

        /// <summary>
        /// The client has yet to provide the gender for their account.
        /// </summary>
        SetGender,

        /// <summary>
        /// The client has yet to enter their registered PIN.
        /// </summary>
        AskPin,

        /// <summary>
        /// The client has yet to register a PIN.
        /// </summary>
        SetPin,

        /// <summary>
        /// The client has authenticated, but hasn't reached World Select yet.
        /// </summary>
        LoggedIn,

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
