using System;
using System.Runtime.Serialization;

namespace OpenStory.Services
{
    /// <summary>
    /// Thrown when there is a problem with configuring a service.
    /// </summary>
    [Serializable]
    public class ServiceConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurationException"/>.
        /// </summary>
        public ServiceConfigurationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurationException"/>.
        /// </summary>
        public ServiceConfigurationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurationException"/>.
        /// </summary>
        public ServiceConfigurationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceConfigurationException"/>.
        /// </summary>
        protected ServiceConfigurationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
