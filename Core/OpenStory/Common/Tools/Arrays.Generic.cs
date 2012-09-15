using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// Generic array helpers!
    /// </summary>
    /// <typeparam name="T">The type of the array.</typeparam>
    public static class Arrays<T>
    {
        /// <summary>
        /// An empty array instance.
        /// </summary>
        public static readonly T[] Empty = new T[0];
    }

}
