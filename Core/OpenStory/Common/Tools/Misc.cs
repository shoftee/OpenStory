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
        /// Sets the specified variable to the default for <typeparamref name="T"/> and returns <see langword="false"/>.
        /// </summary>
        /// <typeparam name="T">The type of the variable.</typeparam>
        /// <param name="value">The variable to set to the default value.</param>
        /// <returns>always <see langword="false"/>.</returns>
        public static bool Fail<T>(out T value)
        {
            value = default(T);
            return false;
        }

        /// <summary>
        /// Wraps the provided list in a <see cref="ReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to wrap.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="list"/> is <see langword="null"/>.
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
        /// <param name="lock">The lock to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lock"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ReadLock(this ReaderWriterLockSlim @lock, Action action)
        {
            if (@lock == null)
            {
                throw new ArgumentNullException("lock");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            @lock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Executes the provided action in a read-lock block.
        /// </summary>
        /// <typeparam name="T">The return type of the callback.</typeparam>
        /// <param name="lock">The lock to use.</param>
        /// <param name="func">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lock"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
        public static T ReadLock<T>(this ReaderWriterLockSlim @lock, Func<T> func)
        {
            if (@lock == null)
            {
                throw new ArgumentNullException("lock");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            @lock.EnterReadLock();
            try
            {
                return func();
            }
            finally
            {
                @lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Executes the provided action in a write-lock block.
        /// </summary>
        /// <param name="lock">The lock to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lock"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void WriteLock(this ReaderWriterLockSlim @lock, Action action)
        {
            if (@lock == null)
            {
                throw new ArgumentNullException("lock");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            @lock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                @lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Executes the provided action in a write-lock block.
        /// </summary>
        /// <typeparam name="T">The return type of the callback.</typeparam>
        /// <param name="lock">The lock to use.</param>
        /// <param name="func">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lock"/> or <paramref name="func"/> is <see langword="null"/>.</exception>
        public static T WriteLock<T>(this ReaderWriterLockSlim @lock, Func<T> func)
        {
            if (@lock == null)
            {
                throw new ArgumentNullException("lock");
            }

            if (func == null)
            {
                throw new ArgumentNullException("func");
            }

            @lock.EnterWriteLock();
            try
            {
                return func();
            }
            finally
            {
                @lock.ExitWriteLock();
            }
        }
    }
}
