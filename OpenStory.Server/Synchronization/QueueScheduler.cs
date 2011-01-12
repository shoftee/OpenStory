using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using OpenStory.Common.Threading;

namespace OpenStory.Server.Synchronization
{
    /// <summary>
    /// A simple thread-safe queue-based IScheduler implementation.
    /// </summary>
    internal class QueueScheduler : IScheduler
    {
        private AtomicBoolean isWorking;
        private ConcurrentQueue<Task> tasks;

        /// <summary>
        /// Initializes a new instance of QueueScheduler.
        /// </summary>
        public QueueScheduler()
        {
            this.isWorking = new AtomicBoolean(false);
            this.tasks = new ConcurrentQueue<Task>();
        }

        #region IScheduler Members

        /// <summary>
        /// Schedules an <see cref="IRunnable" /> to be processed at a later time.
        /// </summary>
        /// <param name="runnable">The IRunnable to schedule for processing.</param>
        public void Schedule(IRunnable runnable)
        {
            if (runnable == null) throw new ArgumentNullException("runnable");

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
            if (action == null) throw new ArgumentNullException("action");

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