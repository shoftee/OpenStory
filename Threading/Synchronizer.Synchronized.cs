using System;
using System.Collections.Concurrent;

namespace OpenMaple.Threading
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

    static partial class Synchronizer
    {
        /// <summary>
        /// Represents a synchronization context around another object.
        /// </summary>
        /// <typeparam name="T">The type of the object to synchronize.</typeparam>
        class Synchronized<T> : ISynchronized<T>, ISchedulable
            where T : class
        {
            private T item;
            private ConcurrentQueue<Action<T>> actions;
            private IScheduler scheduler;

            /// <summary>
            /// Initializes a new Synchronized(T) wrapper around the given object.
            /// </summary>
            /// <param name="item">The item to wrap around.</param>
            /// <param name="scheduler">An execution scheduler for this Synchronized(T).</param>
            /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="item"/> or <paramref name="scheduler"/> are null.</exception>
            public Synchronized(T item, IScheduler scheduler)
            {
                if (item == null) throw new ArgumentNullException("item");
                if (scheduler == null) throw new ArgumentNullException("scheduler");

                this.item = item;
                this.scheduler = scheduler;

                this.actions = new ConcurrentQueue<Action<T>>();
            }

            /// <summary>
            /// Schedules the given action for execution.
            /// </summary>
            /// <param name="action">The action to execute.</param>
            public void Schedule(Action<T> action)
            {
                this.actions.Enqueue(action);
                this.scheduler.EnqueueObject(this);
            }

            public Action GetPendingAction()
            {
                Action<T> objectAction;
                if (!actions.TryDequeue(out objectAction))
                {
                    return null;
                }
                return GetAction(objectAction);
            }

            private Action GetAction(Action<T> objectAction)
            {
                if (!this.actions.IsEmpty)
                {
                    return () => this.ExecuteAndEnqueueAgain(objectAction);
                }
                else
                {
                    return () => objectAction(item);
                }
            }

            private void ExecuteAndEnqueueAgain(Action<T> action)
            {
                action(item);
                this.scheduler.EnqueueObject(this);
            }
        }
    }
}
