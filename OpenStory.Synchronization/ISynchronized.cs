using System;

namespace OpenStory.Synchronization
{
    /// <summary>
    /// Provides basic synchronization methods for an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to synchronize.</typeparam>
    public interface ISynchronized<out T>
        where T : class
    {
        /// <summary>
        /// Schedules an <see cref="System.Action{T}"/> to be executed on this object.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        void Schedule(Action<T> action);
    }
}
