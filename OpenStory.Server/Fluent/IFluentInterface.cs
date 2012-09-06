using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Hides the default <see cref="Object"/> members from Intellisense.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}
