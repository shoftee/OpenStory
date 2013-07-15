using System;
using System.Collections.Generic;

namespace OpenStory.Server.Modules.Config
{
    /// <summary>
    /// Represents a collection of configuration entries for a configuration domain.
    /// </summary>
    public sealed class DomainConfig
    {
        private readonly string domainName;
        private readonly IConfigDataProvider provider;

        internal DomainConfig(string name, IConfigDataProvider provider)
        {
            this.domainName = name;
            this.provider = provider;
        }

        /// <summary>
        /// Gets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the configuration entry.</param>
        /// <param name="throwOnMissing">Whether to throw an exception when the key is not in the collection.</param>
        /// <returns>
        /// if the key corresponds to a valid entry, the value of the entry cast to to <typeparamref name="T"/>;
        /// otherwise, if <paramref name="throwOnMissing"/> was false, the default value for <typeparamref name="T"/>.
        /// </returns>
        public T Get<T>(string key, bool throwOnMissing = false)
        {
            object value = this.provider.GetObject(this.domainName, key);
            if (value != null)
            {
                return (T)value;
            }
            else if (!throwOnMissing)
            {
                return default(T);
            }
            else
            {
                string format = "The configuration key '{0}' did not correspond to an existing entry.";
                string message = string.Format(format, key);
                throw new KeyNotFoundException(message);
            }
        }

        /// <summary>
        /// Sets the value for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the configuration entry.</param>
        /// <param name="newValue">The new value for the configuration entry.</param>
        public void Set<T>(string key, T newValue)
        {
            this.provider.StoreObject(this.domainName, key, newValue);
        }
    }
}