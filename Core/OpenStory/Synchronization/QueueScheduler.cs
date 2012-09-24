using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using OpenStory.Common;

namespace OpenStory.Synchronization
{
    /// <summary>
    /// A simple thread-safe queue-based <see cref="IScheduler"/> implementation.
    /// </summary>
    internal sealed class QueueScheduler : IScheduler
    {
        private readonly AtomicBoolean isWorking;
        private readonly ConcurrentQueue<Task> tasks;

        /// <summary>
        /// Initializes a new instance of <see cref="QueueScheduler"/>.
        /// </summary>
        public QueueScheduler()
        {
            this.isWorking = new AtomicBoolean(false);
            this.tasks = new ConcurrentQueue<Task>();
        }

        #region IScheduler Members

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="runnable"/> is <c>null</c>.
        /// </exception>
        public void Schedule(IRunnable runnable)
        {
            if (runnable == null)
            {
                throw new ArgumentNullException("runnable");
            }

            this.ScheduleActionInternal(runnable.Run);
            this.ExecutePending();
        }

        #endregion

        /// <summary>
        /// Schedules a simple action for execution. 
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        public void ScheduleAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.ScheduleActionInternal(action);
            this.ExecutePending();
        }

        private void ScheduleActionInternal(Action action)
        {
            this.tasks.Enqueue(new Task(action));
        }

        private void ExecutePending()
        {
            // If we're already working, go away.
            if (this.isWorking.CompareExchange(false, true))
            {
                return;
            }

            Task task;
            while (this.tasks.TryDequeue(out task))
            {
                task.Start();
            }

            this.isWorking.Exchange(false);
        }
    }
}
