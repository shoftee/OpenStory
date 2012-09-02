using System;

namespace OpenStory.Server.Diagnostics
{
    internal sealed class DefaultLogger : ILogger
    {
        /// <inheritdoc />
        public void Info(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine("[Info] " + format, args);
            }
        }

        /// <inheritdoc />
        public void Warning(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine("[Warning] " + format, args);
            }
        }

        /// <inheritdoc />
        public void Error(string format, params object[] args)
        {
            lock (Console.Out)
            {
                Console.WriteLine("[Error] " + format, args);
            }
        }
    }
}