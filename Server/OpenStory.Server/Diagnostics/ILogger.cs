using OpenStory.Server.Fluent;

namespace OpenStory.Server.Diagnostics
{
    /// <summary>
    /// Provides methods for logging messages.
    /// </summary>
    public interface ILogger : IFluentInterface
    {
        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void Info(string format, params object[] args);

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void Warning(string format, params object[] args);

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void Error(string format, params object[] args);
    }
}
