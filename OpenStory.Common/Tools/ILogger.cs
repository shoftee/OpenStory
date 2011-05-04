namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Provides methods for logging messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="infoString">The string to write.</param>
        void WriteInfo(string infoString);

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteInfo(string format, params object[] args);

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="warningString">The string to write.</param>
        void WriteWarning(string warningString);

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteWarning(string format, params object[] args);

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="errorString">The string to write.</param>
        void WriteError(string errorString);

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteError(string format, params object[] args);
      }
 }