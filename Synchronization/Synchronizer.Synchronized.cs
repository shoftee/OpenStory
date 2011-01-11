using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OpenMaple.Synchronization
{
    static partial class Synchronizer
    {
        /// <summary>
        /// Represents a synchronization context around another object.
        /// </summary>
        /// <typeparam name="T">The type of the object to synchronize.</typeparam>
        class Synchronized<T> : ISynchronized<T>, IRunnable
            where T : class
        {
            private T item;
            private ConcurrentQueue<Action<T>> actions;
            private IScheduler scheduler;

            /// <summary>
            /// Initializes a new Synchronized(T) wrapper around the given object.
            /// </summary>
            /// <param name="item">The item to wrap around.</param>
            /// <param name="scheduler">An execution scheduler to use for this Synchronized(T).</param>
            /// <exception cref="ArgumentNullException">
            /// The exception is thrown if <paramref name="item"/> or <paramref name="scheduler"/> are null.
            /// </exception>
            public Synchronized(T item, IScheduler scheduler)
            {
                if (item == null) throw new ArgumentNullException("item");
                if (scheduler == null) throw new ArgumentNullException("scheduler");

                this.item = item;
                this.scheduler = scheduler;

                this.actions = new ConcurrentQueue<Action<T>>();
            }

            /// <summary>
            /// Schedules the given action for execution with the Synchronized(T).
            /// </summary>
            /// <param name="action">The action to execute.</param>
            /// <exception cref="ArgumentNullException">
            /// The exception is thrown if <paramref name="action"/> is null.
            /// </exception>
            public void Schedule(Action<T> action)
            {
                if (action == null) throw new ArgumentNullException("action");

                this.actions.Enqueue(action);
                this.scheduler.Schedule(this);
            }

            /// <summary>
            /// Runs a pending action for the Synchronized(T).
            /// </summary>
            /// <exception cref="InvalidOperationException">
            /// The exception is thrown when there is no pending action.
            /// </exception>
            public void Run()
            {
                Action<T> objectAction;
                if (!actions.TryDequeue(out objectAction))
                {
                    throw new InvalidOperationException("There is no pending action for this IRunnable.");
                }

                Task.Factory.StartNew(this.GetAction(objectAction));
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
                this.scheduler.Schedule(this);
            }
        }
    }
}
