using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides extension methods. Which are useful. For stuff.
    /// </summary>
    public static class Extensions
    {
        public static IDisposable AsDisposable(this object obj)
        {
            return obj as IDisposable;
        }
    }
}
