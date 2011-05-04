namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// Specifies an ordering for initialization of server modules.
    /// </summary>
    public enum InitializationStage
    {
        /// <summary>
        /// Will be initialized first.
        /// </summary>
        StartUp = 0,
        /// <summary>
        /// Will be initialized after <see cref="StartUp"/> and before <see cref="Storage"/>.
        /// </summary>
        Settings = 1,
        /// <summary>
        /// Will be initialized after <see cref="Settings"/> and before <see cref="Worlds"/>.
        /// </summary>
        Storage = 2,
        /// <summary>
        /// Will be initialized after <see cref="Storage"/> and before <see cref="Login"/>.
        /// </summary>
        Worlds = 3,
        /// <summary>
        /// Will be initialized after <see cref="Worlds"/>.
        /// </summary>
        Login = 4,
      }
 }