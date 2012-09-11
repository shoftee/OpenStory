using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Diagnostics
{
    /// <summary>
    /// Represents an <see cref="ILogger"/> implementation that does nothing.
    /// </summary>
    internal sealed class NullLogger : ILogger
    {
        /// <summary>
        /// The singleton instance of the <see cref="NullLogger"/> type.
        /// </summary>
        public static readonly NullLogger Instance = new NullLogger();
        private NullLogger() { }

        /// <inheritdoc />
        /// <remarks>
        /// This method does nothing.
        /// </remarks>
        public void Info(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method does nothing.
        /// </remarks>
        public void Warning(string format, params object[] args)
        {
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method does nothing.
        /// </remarks>
        public void Error(string format, params object[] args)
        {
        }
    }
}
