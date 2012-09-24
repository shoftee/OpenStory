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
        private readonly Dictionary<string, string> data;

        /// <summary>
        /// Gets configuration values by their key.
        /// </summary>
        /// <param name="key">The key for the configuration value.</param>
        public string this[string key]
        {
            get
            {
                string value;
                this.data.TryGetValue(key, out value);
                return value;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfiguration"/>.
        /// </summary>
        /// <param name="parameters">The configuration parameters to initalize the instance with.</param>
        public ServiceConfiguration(IDictionary<string, string> parameters)
        {
            this.data = new Dictionary<string, string>(parameters);
        }
    }
}