namespace OpenStory.Server
{
    /// <summary>
    /// Constants for the server emulator.
    /// </summary>
    public static class ServerConstants
    {
        /// <summary>
        /// The URI of the Authentication server used for inter-process communication.
        /// </summary>
        public const string AuthServiceUri = "net.pipe://localhost/OpenStory/AuthService";

        /// <summary>
        /// The URI of the Account service used for inter-process communication.
        /// </summary>
        public const string AccountServiceUri = "net.pipe://localhost/OpenStory/AccountService";
    }
}