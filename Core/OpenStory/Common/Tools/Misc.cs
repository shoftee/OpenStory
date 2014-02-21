using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace OpenStory.Common
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
        /// Exchanges the provided reference with <see langword="null"/> and then disposes the previously referenced object.
        /// </summary>
        public static void AssignNullAndDispose<TDisposable>(ref TDisposable disposable)
            where TDisposable : class, IDisposable
        {
            var value = Interlocked.Exchange(ref disposable, null);
            if (value != null)
            {
                value.Dispose();
            }
        }

        /// <summary>
        /// Executes the provided action in a read-lock block.
        /// </summary>
        /// <param name="lock">The lock to use.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lock"/> or <paramref name="action"/> is <see langword="null"/>.</exception>
        public static void ReadLock(this ReaderWriterLockSlim @lock, Action action)
        {
            Guard.NotNull(() => @lock, @lock);
            Guard.NotNull(() => action, action);

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
            Guard.NotNull(() => @lock, @lock);
            Guard.NotNull(() => func, func);

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
            Guard.NotNull(() => @lock, @lock);
            Guard.NotNull(() => action, action);

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
            Guard.NotNull(() => @lock, @lock);
            Guard.NotNull(() => func, func);

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
