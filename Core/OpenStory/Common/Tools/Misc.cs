using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace OpenStory.Common.Tools
{
    /// <summary>
    /// A static class containing a collection of utility methods that don't have a place to call home :(
    /// </summary>
    public static class Misc
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

        /// <summary>
        /// Wraps the provided list in a <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to wrap.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="list"/> is <c>null</c>.
        /// </exception>
        /// <returns>an instance of <see cref="ReadOnlyCollection{T}"/>.</returns>
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return new ReadOnlyCollection<T>(list);
        }

        /// <summary>
        /// Executes the provided action in a read-lock block.
        /// </summary>
        /// <param name="rwLock">The lock to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rwLock"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static void ReadLock(this ReaderWriterLockSlim rwLock, Action action)
        {
            if (rwLock == null)
            {
                throw new ArgumentNullException("rwLock");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            rwLock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Executes the provided action in a read-lock block.
        /// </summary>
        /// <typeparam name="T">The return type of the callback.</typeparam>
        /// <param name="rwLock">The lock to use.</param>
        /// <param name="func">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rwLock"/> or <paramref name="func"/> is <c>null</c>.</exception>
        public static T ReadLock<T>(this ReaderWriterLockSlim rwLock, Func<T> func)
        {
            if (rwLock == null)
            {
                throw new ArgumentNullException("rwLock");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            rwLock.EnterReadLock();
            try
            {
                return func();
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Executes the provided action in a write-lock block.
        /// </summary>
        /// <param name="rwLock">The lock to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rwLock"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static void WriteLock(this ReaderWriterLockSlim rwLock, Action action)
        {
            if (rwLock == null)
            {
                throw new ArgumentNullException("rwLock");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            rwLock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Executes the provided action in a write-lock block.
        /// </summary>
        /// <typeparam name="T">The return type of the callback.</typeparam>
        /// <param name="rwLock">The lock to use.</param>
        /// <param name="func">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rwLock"/> or <paramref name="func"/> is <c>null</c>.</exception>
        public static T WriteLock<T>(this ReaderWriterLockSlim rwLock, Func<T> func)
        {
            if (rwLock == null)
            {
                throw new ArgumentNullException("rwLock");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            rwLock.EnterWriteLock();
            try
            {
                return func();
            }
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }
}
