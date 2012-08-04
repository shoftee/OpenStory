using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// A static class containing a collection of utility methods that don't have a place to call home :(
    /// </summary>
    public static class MiscTools
    {
        /// <summary>
        /// Sets the specified variable to the default for <typeparamref name="T"/> and returns <c>false</c>.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="value">The variable to set to the default value.</param>
        /// <returns>always <c>false</c>.</returns>
        public static bool Fail<T>(out T value)
        {
            value = default(T);
            return false;
        }

        /// <summary>
        /// Sets the specified variable to the default for <typeparamref name="T"/> and returns <paramref name="result"/>.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="value">The variable to set to the default value.</param>
        /// <param name="result">The value to return.</param>
        /// <returns>always <paramref name="result"/>.</returns>
        public static TResult FailWithResult<T, TResult>(out T value, TResult result)
        {
            value = default(T);
            return result;
        }
    }
}
