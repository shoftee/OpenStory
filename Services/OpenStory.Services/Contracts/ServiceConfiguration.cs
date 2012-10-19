using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
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
        public object this[string key]
        {
            get
            {
                object value;
                this.data.TryGetValue(key, out value);
                return value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfiguration"/>.
        /// </summary>
        /// <param name="parameters">The configuration parameters to initalize the instance with.</param>
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
        /// Thrown if <paramref name="throwIfMissing"/> is <c>true</c> and the <paramref name="key"/> does not correspond to an existing entry.
        /// </exception>
        /// <returns>the value of the found entry cast to <typeparamref name="T"/>, or the default value for the type.</returns>
        public T Get<T>(string key, bool throwIfMissing = false)
        {
            object value;
            if (!this.data.TryGetValue(key, out value))
            {
                if (!throwIfMissing)
                {
                    return default(T);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                return (T)value;
            }
        }
    }
}