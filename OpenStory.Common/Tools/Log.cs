using System;
using System.IO;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Logger class.
    /// </summary>
    public class Log : ILogger
    {
        private static readonly ILogger InternalInstance = new Log();
        private TextWriter writer;

        private Log()
        {
            this.writer = Console.Out;
        }

        /// <summary>
        /// An <see cref="ILogger"/> instance of the singleton Log class.
        /// </summary>
        public static ILogger Instance
        {
            get { return InternalInstance; }
        }

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="info">The string to write.</param>
        public static void WriteInfo(string info)
        {
            InternalInstance.WriteInfo(info);
        }

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteInfo(string format, params object[] args)
        {
            InternalInstance.WriteInfo(format, args);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="warning">The string to write.</param>
        public static void WriteWarning(string warning)
        {
            InternalInstance.WriteWarning(warning);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteWarning(string format, params object[] args)
        {
            InternalInstance.WriteWarning(format, args);
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="error">The string to write.</param>
        public static void WriteError(string error)
        {
            InternalInstance.WriteError(error);
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteError(string format, params object[] args)
        {
            InternalInstance.WriteError(format, args);
        }

        #region Explicit ILogger implementation

        void ILogger.WriteInfo(string info)
        {
            lock (this.writer) this.writer.WriteLine("[Info] {0}", info);
        }

        void ILogger.WriteInfo(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Info] " + format, args);
        }

        void ILogger.WriteWarning(string warning)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] {0}", warning);
        }

        void ILogger.WriteWarning(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] " + format, args);
        }

        void ILogger.WriteError(string error)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] {0}", error);
        }

        void ILogger.WriteError(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] " + format, args);
        }

        #endregion
    }

    /// <summary>
    /// Provides methods for logging messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="info">The string to write.</param>
        void WriteInfo(string info);
        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteInfo(string format, params object[] args);

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="warning">The string to write.</param>
        void WriteWarning(string warning);
        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteWarning(string format, params object[] args);

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="error">The string to write.</param>
        void WriteError(string error);
        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        void WriteError(string format, params object[] args);
    }
}