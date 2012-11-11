using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Contains definitions for service settings.
    /// </summary>
    public static class ServiceSettings
    {
        /// <summary>
        /// The constant used as the service type setting key.
        /// </summary>
        public const string ServiceTypeKey = "ServiceType";

        /// <summary>
        /// Contains definitions for service URI settings.
        /// </summary>
        public static class Uri
        {
            /// <summary>
            /// The constant used as the service URI setting key.
            /// </summary>
            public const string Key = "ServiceUri";
        }

        /// <summary>
        /// Contains definitions for auth service settings.
        /// </summary>
        public static class Auth
        {
            /// <summary>
            /// The constant used as the authentication service type setting value.
            /// </summary>
            public const string ServiceType = "Auth";

            /// <summary>
            /// The template for the auth service configurations.
            /// </summary>
            internal static readonly IDictionary<string, object> Template =
                new Dictionary<string, object>()
                {
                    { ServiceTypeKey, ServiceType },
                    { Uri.Key, "" },
                };
        }

        /// <summary>
        /// Contains definitions for world service settings.
        /// </summary>
        public static class World
        {
            /// <summary>
            /// The constant used as the world service type setting value.
            /// </summary>
            public const string ServiceType = "World";

            /// <summary>
            /// The constant used as the world identifier setting key.
            /// </summary>
            public const string Id = "WorldId";

            /// <summary>
            /// The constant used as the channel count setting key.
            /// </summary>
            public const string ChannelCount = "ChannelCount";

            /// <summary>
            /// The template for the world service configurations.
            /// </summary>
            internal static readonly IDictionary<string, object> Template =
                new Dictionary<string, object>()
                {
                    { ServiceTypeKey, ServiceType },
                    { Uri.Key, "" },
                    { Id, -1 },
                    { ChannelCount, -1 },
                };
        }

        /// <summary>
        /// Contains definitions for world service settings.
        /// </summary>
        public static class Channel
        {
            /// <summary>
            /// The constant used as the channel service type setting value.
            /// </summary>
            public const string ServiceType = "Channel";

            /// <summary>
            /// The constant used as the channel identifier setting key.
            /// </summary>
            public const string WorldId = "ContainingWorldId";

            /// <summary>
            /// The constant used as the channel identifier setting key.
            /// </summary>
            public const string ChannelId = "ChannelId";

            /// <summary>
            /// The constant used as the player capacity setting key.
            /// </summary>
            public const string PlayerCapacity = "PlayerCapacity";

            /// <summary>
            /// The template for the channel service configurations.
            /// </summary>
            internal static readonly IDictionary<string, object> Template =
                new Dictionary<string, object>()
                {
                    { ServiceTypeKey, ServiceType },
                    { Uri.Key, "" },
                    { ChannelId, -1 },
                    { PlayerCapacity, -1 },
                };
        }
    }
}
