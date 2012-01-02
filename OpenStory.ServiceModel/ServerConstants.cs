using System;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Constants for the server emulator.
    /// </summary>
    public static class ServerConstants
    {
        /// <summary>
        /// URI constants.
        /// </summary>
        public static class Uris
        {
            /// <summary>
            /// The URI for the OpenStory WCF universe service.
            /// </summary>
            public static readonly Uri UniverseService = new Uri("net.tcp://localhost:10000/OpenStory/UniverseService");

            /// <summary>
            /// The URI of the OpenStory WCF account service.
            /// </summary>
            public static readonly Uri AccountService = new Uri("net.tcp://localhost:10001/OpenStory/AccountService");

            /// <summary>
            /// The URI of the OpenStory WCF authentication service.
            /// </summary>
            public static readonly Uri AuthService = new Uri("net.tcp://localhost:10002/OpenStory/AuthService");

        }
    }
}