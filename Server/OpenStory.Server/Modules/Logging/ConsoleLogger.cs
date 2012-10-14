using System;

namespace OpenStory.Server.Modules.Logging
{
    /// <summary>
    /// Represents a simple console logger.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        /// <inheritdoc />
        public void Info(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine(Strings.ConsoleLoggerInfoPrefix + @" " + format, args);
            }
        }

        /// <inheritdoc />
        public void Warning(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine(Strings.ConsoleLoggerWarningPrefix + @" " + format, args);
            }
        }

        /// <inheritdoc />
        public void Error(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine(Strings.ConsoleLoggerErrorPrefix + @" " + format, args);
            }
        }
    }
}