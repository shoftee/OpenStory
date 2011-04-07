using System;
using System.IO;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Logger class.
    /// </summary>
    public class Log : ILogger
    {
        private static readonly Log InternalInstance = new Log();
        private TextWriter writer;

        private Log()
        {
            this.writer = Console.Out;
        }

        /// <summary>
        /// Gets or sets the output stream for the Log.
        /// </summary>
        /// <remarks>
        /// The default writer is <see cref="Console.Out"/>.</remarks>
        public static TextWriter Writer
        {
            get { return InternalInstance.writer; }
            set
            {
                if (value == null) throw new InvalidOperationException("The Log writer cannot be set to null.");
                else InternalInstance.writer = value;
            } 
        }

        /// <summary>
        /// Gets the <see cref="ILogger"/> instance of the singleton Log class.
        /// </summary>
        public static ILogger Instance
        {
            get { return InternalInstance; }
        }

        #region Static methods

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="infoString">The string to write.</param>
        public static void WriteInfo(string infoString)
        {
            Instance.WriteInfo(infoString);
        }

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteInfo(string format, params object[] args)
        {
            Instance.WriteInfo(format, args);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="warningString">The string to write.</param>
        public static void WriteWarning(string warningString)
        {
            Instance.WriteWarning(warningString);
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteWarning(string format, params object[] args)
        {
            Instance.WriteWarning(format, args);
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="errorString">The string to write.</param>
        public static void WriteError(string errorString)
        {
            Instance.WriteError(errorString);
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <param name="format">The format of the message to write.</param>
        /// <param name="args">The arguments to fill into the message format.</param>
        public static void WriteError(string format, params object[] args)
        {
            Instance.WriteError(format, args);
        }

        #endregion

        #region Explicit ILogger implementation

        void ILogger.WriteInfo(string infoString)
        {
            lock (this.writer) this.writer.WriteLine("[Info] {0}", infoString);
        }

        void ILogger.WriteInfo(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Info] " + format, args);
        }

        void ILogger.WriteWarning(string warningString)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] {0}", warningString);
        }

        void ILogger.WriteWarning(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Warning] " + format, args);
        }

        void ILogger.WriteError(string errorString)
        {
            lock (this.writer) this.writer.WriteLine("[Error] {0}", errorString);
        }

        void ILogger.WriteError(string format, params object[] args)
        {
            lock (this.writer) this.writer.WriteLine("[Error] " + format, args);
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