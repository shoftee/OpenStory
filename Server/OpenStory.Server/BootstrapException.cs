using System;

namespace OpenStory.Server
{
    /// <summary>
    /// An exception thrown when there is an error during bootstrapping.
    /// </summary>
    [Serializable]
    public sealed class BootstrapException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapException"/>.
        /// </summary>
        public BootstrapException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BootstrapException"/>.
        /// </summary>
        public BootstrapException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
