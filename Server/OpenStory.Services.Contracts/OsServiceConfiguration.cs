using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a configuration for a game service.
    /// </summary>
    [DataContract]
    public sealed class OsServiceConfiguration
    {
        [DataMember]
        private readonly Dictionary<string, object> _data;

        /// <summary>
        /// Initializes a new instance of the <see cref="OsServiceConfiguration"/> class.
        /// </summary>
        /// <param name="parameters">The configuration parameters to initialize the instance with.</param>
        public OsServiceConfiguration(IDictionary<string, object> parameters)
        {
            _data = new Dictionary<string, object>(parameters);
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
            if (_data.TryGetValue(key, out value))
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
            if (_data.TryGetValue(key, out value))
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
    }
}