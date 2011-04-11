using System;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Constants for the server emulator.
    /// </summary>
    public static class ServerConstants
    {
        /// <summary>
        /// The URI of the Authentication server used for inter-process communication.
        /// </summary>
        public static readonly Uri AuthServiceUri = new Uri("net.pipe://localhost/OpenStory/AuthService");

        /// <summary>
        /// The URI of the Account service used for inter-process communication.
        /// </summary>
        public static readonly Uri AccountServiceUri = new Uri("net.pipe://localhost/OpenStory/AccountService");
    }
}