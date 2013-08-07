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
        /// Represents a synchronization wrapper around another object.
        /// </summary>
        /// <typeparam name="T">The type of the object to synchronize.</typeparam>
        private sealed class Synchronized<T> : ISynchronized<T>, IRunnable
            where T : class
        {
            private readonly ConcurrentQueue<Action<T>> actions;
            private readonly T obj;
            private readonly IScheduler scheduler;

            /// <summary>
            /// Initializes a new instance of <see cref="Synchronized{T}"/> around the specified object.
            /// </summary>
            /// <param name="obj">The object to wrap.</param>
            /// <param name="scheduler">An execution scheduler to use.</param>
            /// <exception cref="ArgumentNullException">
            /// Thrown if either of <paramref name="obj"/> or <paramref name="scheduler"/> is <see langword="null"/>.
            /// </exception>
            public Synchronized(T obj, IScheduler scheduler)
            {
                if (obj == null)
                {
                    throw new ArgumentNullException("obj");
                }

                if (scheduler == null)
                {
                    throw new ArgumentNullException("scheduler");
                }

                this.obj = obj;
                this.scheduler = scheduler;

                this.actions = new ConcurrentQueue<Action<T>>();
            }

            #region IRunnable Members

            /// <summary>
            /// Runs a the next pending action for this instance.
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// Thrown if there is no pending action.
            /// </exception>
            public void Run()
            {
                Action<T> objectAction;
                if (!this.actions.TryDequeue(out objectAction))
                {
                    return;
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
            /// Thrown if <paramref name="action"/> is <see langword="null"/>.
            /// </exception>
            public void Schedule(Action<T> action)
            {
                if (action == null)
                {
                    throw new ArgumentNullException("action");
                }

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
                    return () => objectAction(this.obj);
                }
            }

            private void ExecuteAndEnqueueAgain(Action<T> action)
            {
                action(this.obj);
                this.scheduler.Schedule(this);
            }
        }

        #endregion
    }
}
