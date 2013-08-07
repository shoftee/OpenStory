using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Represents a configuration for a game service.
    /// </summary>
    [DataContract]
    public sealed class ServiceConfiguration
    {
        [DataMember]
        private readonly Dictionary<string, object> data;

        /// <summary>
        /// Gets configuration values by their key.
        /// </summary>
        /// <param name="key">The key for the configuration value.</param>
        private object this[string key]
        {
            get
            {
                object value;
                this.data.TryGetValue(key, out value);
                return value;
            }
            set
            {
                this.data[key] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConfiguration"/> class.
        /// </summary>
        /// <param name="parameters">The configuration parameters to initialize the instance with.</param>
        public ServiceConfiguration(IDictionary<string, object> parameters)
        {
            this.data = new Dictionary<string, object>(parameters);
        }

        /// <summary>
        /// Retrieves the value for a key and casts it to a type.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key of the entry to retrieve.</param>
        /// <param name="throwIfMissing">Whether to throw an exception if an entry is not found.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if <paramref name="throwIfMissing"/> is <see langword="true"/> and the <paramref name="key"/> does not correspond to an existing entry.
        /// </exception>
        /// <returns>the value of the found entry cast to <typeparamref name="T"/>, or the default value for the type.</returns>
        public T Get<T>(string key, bool throwIfMissing = false)
        {
            object value;
            if (this.data.TryGetValue(key, out value))
            {
                return (T)value;
            }

            if (!throwIfMissing)
            {
                return default(T);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Retrieves the value for a key and casts it to a type.
        /// </summary>
        /// <typeparam name="T">The type to cast the value to.</typeparam>
        /// <param name="key">The key of the entry to retrieve.</param>
        /// <param name="throwIfMissing">Whether to throw an exception if an entry is not found.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if <paramref name="throwIfMissing"/> is <see langword="true"/> and the <paramref name="key"/> does not correspond to an existing entry.
        /// </exception>
        /// <returns>the value of the found entry cast to <typeparamref name="T"/>, or the default value for <see cref="Nullable{T}"/>.</returns>
        public T? GetValue<T>(string key, bool throwIfMissing = false)
            where T : struct
        {
            object value;
            if (this.data.TryGetValue(key, out value))
            {
                return (T)value;
            }

            if (!throwIfMissing)
            {
                return default(T?);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Creates a service configuration for an auth service.
        /// </summary>
        public static ServiceConfiguration Auth(Uri uri)
        {
            var config = new ServiceConfiguration(ServiceSettings.Auth.Template);
            config[ServiceSettings.Uri.Key] = uri;
            return config;
        }

        /// <summary>
        /// Creates a service configuration for an account service.
        /// </summary>
        public static ServiceConfiguration Account(Uri uri)
        {
            var config = new ServiceConfiguration(ServiceSettings.Account.Template);
            config[ServiceSettings.Uri.Key] = uri;
            return config;
        }

        /// <summary>
        /// Creates a service configuration for an world service.
        /// </summary>
        public static ServiceConfiguration World(Uri uri, int worldId, int channelCount = 6)
        {
            var config = new ServiceConfiguration(ServiceSettings.World.Template);
            config[ServiceSettings.Uri.Key] = uri;
            config[ServiceSettings.World.Id] = worldId;
            config[ServiceSettings.World.ChannelCount] = channelCount;
            return config;
        }

        /// <summary>
        /// Creates a service configuration for an channel service.
        /// </summary>
        public static ServiceConfiguration Channel(Uri uri, int worldId, int channelId, int playerCapacity = 100)
        {
            var config = new ServiceConfiguration(ServiceSettings.Channel.Template);
            config[ServiceSettings.Uri.Key] = uri;
            config[ServiceSettings.Channel.WorldId] = worldId;
            config[ServiceSettings.Channel.ChannelId] = channelId;
            config[ServiceSettings.Channel.PlayerCapacity] = playerCapacity;
            return config;
        }
    }
}