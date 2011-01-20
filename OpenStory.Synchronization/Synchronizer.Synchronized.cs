using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OpenStory.Synchronization
{
    /// <summary>
    /// Provides static methods for global action synchronization.
    /// </summary>
    public static partial class Synchronizer
    {
        #region Nested type: Synchronized

        /// <summary>
        /// Represents a synchronization context around another object.
        /// </summary>
        /// <typeparam name="T">The type of the object to synchronize.</typeparam>
        private class Synchronized<T> : ISynchronized<T>, IRunnable
            where T : class
        {
            private ConcurrentQueue<Action<T>> actions;
            private T item;
            private IScheduler scheduler;

            /// <summary>
            /// Initializes a new Synchronized(T) wrapper around the given object.
            /// </summary>
            /// <param name="item">The item to wrap around.</param>
            /// <param name="scheduler">An execution scheduler to use for this Synchronized(T).</param>
            /// <exception cref="ArgumentNullException">
            /// Thrown if <paramref name="item"/> or <paramref name="scheduler"/> are null.
            /// </exception>
            public Synchronized(T item, IScheduler scheduler)
            {
                if (item == null) throw new ArgumentNullException("item");
                if (scheduler == null) throw new ArgumentNullException("scheduler");

                this.item = item;
                this.scheduler = scheduler;

                this.actions = new ConcurrentQueue<Action<T>>();
            }

            #region IRunnable Members

            /// <summary>
            /// Runs a pending action for the Synchronized(T).
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// Thrown if there is no pending action.
            /// </exception>
            public void Run()
            {
                Action<T> objectAction;
                if (!this.actions.TryDequeue(out objectAction))
                {
                    throw new InvalidOperationException("There is no pending action for this IRunnable.");
                }

                Task.Factory.StartNew(this.GetAction(objectAction));
            }

            #endregion

            #region ISynchronized<T> Members

            /// <summary>
            /// Schedules the given action for execution with the Synchronized(T).
            /// </summary>
            /// <param name="action">The action to execute.</param>
            /// <exception cref="ArgumentNullException">
            /// Thrown if <paramref name="action"/> is <c>null</c>.
            /// </exception>
            public void Schedule(Action<T> action)
            {
                if (action == null) throw new ArgumentNullException("action");

                this.actions.Enqueue(action);
                this.scheduler.Schedule(this);
            }

            #endregion

            private Action GetAction(Action<T> objectAction)
            {
                if (!this.actions.IsEmpty)
                {
                    return () => this.ExecuteAndEnqueueAgain(objectAction);
                }
                else
                {
                    return () => objectAction(this.item);
                }
            }

            private void ExecuteAndEnqueueAgain(Action<T> action)
            {
                action(this.item);
                this.scheduler.Schedule(this);
            }
        }

        #endregion
    }
}