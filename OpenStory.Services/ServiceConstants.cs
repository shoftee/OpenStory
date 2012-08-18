using System;

namespace OpenStory.Services
{
    /// <summary>
    /// Constants for the server emulator.
    /// </summary>
    public static class ServiceConstants
    {
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

            /// <summary>
            /// Gets the URI for the specified OpenStory WCF channel service.
            /// </summary>
            /// <param name="worldId">The ID of the world.</param>
            /// <param name="channelId">The ID of the channel.</param>
            /// <returns>the requested URI.</returns>
            public static Uri GetChannelService(int worldId, int channelId)
            {
                int port = 10101 + worldId * 100 + channelId;

                string uri = String.Format("net.tcp://localhost:{0}/OpenStory/ChannelService", port);
                return new Uri(uri);
            }

            /// <summary>
            /// Gets the URI for the specified OpenStory WCF world service.
            /// </summary>
            /// <param name="worldId">The ID of the world.</param>
            /// <returns>the requested URI.</returns>
            public static Uri GetWorldService(int worldId)
            {
                int port = 10100 + worldId * 100;

                string uri = String.Format("net.tcp://localhost:{0}/OpenStory/WorldService", port);
                return new Uri(uri);
            }
        }
    }
}