using System;

namespace OpenMaple.Synchronization
{
    /// <summary>
    /// Provides basic synchronization methods for an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to synchronize.</typeparam>
    interface ISynchronized<out T>
        where T : class
    {
        void Schedule(Action<T> action);
    }
}