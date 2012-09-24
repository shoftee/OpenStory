using System;

namespace OpenStory.Services
{
    /// <summary>
    /// Constants for the server emulator.
    /// </summary>
    public static class ServiceConstants
    {
        // TODO: This class is pretty useless, get rid of it?
        
        #region Nested type: Uris

        /// <summary>
        /// URI constants.
        /// </summary>
        public static class Uris
        {
            /// <summary>
            /// The URI for the OpenStory WCF nexus service.
            /// </summary>
            public static readonly Uri NexusService = new Uri("net.tcp://localhost:10000/OpenStory/NexusService");

            /// <summary>
            /// The URI of the OpenStory WCF account service.
            /// </summary>
            public static readonly Uri AccountService = new Uri("net.tcp://localhost:10001/OpenStory/AccountService");

            /// <summary>
            /// The URI of the OpenStory WCF authentication service.
            /// </summary>
            public static readonly Uri AuthService = new Uri("net.tcp://localhost:10002/OpenStory/AuthService");
        }

        #endregion
    }
}
