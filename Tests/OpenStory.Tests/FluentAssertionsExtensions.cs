using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Tests
{
    internal static class FluentAssertionsExtensions
    {
        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <remarks>
        /// This method is used to ignore the return value of an expression easily.
        /// </remarks>
        /// <param name="obj">Some object.</param>
        public static void Whatever(this object obj)
        {
        }
    }
}
