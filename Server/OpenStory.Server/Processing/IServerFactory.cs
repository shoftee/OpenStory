using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Provides methods for creating server instances.
    /// </summary>
    public interface IServerFactory
    {
        /// <summary>
        /// Creates a new <see cref="IServerOperator"/> instance.
        /// </summary>
        IServerProcess CreateProcess();

        /// <summary>
        /// Creates a new <see cref="IServerOperator"/> instance.
        /// </summary>
        IServerOperator CreateOperator();
    }
}
