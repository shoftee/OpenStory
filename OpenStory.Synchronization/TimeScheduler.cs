using System;
using System.Threading;

namespace OpenStory.Synchronization
{
    internal sealed partial class TimeScheduler
    {
        private Timeline timeline;

        public TimeScheduler()
        {
            this.timeline = new Timeline();
        }

        /// <summary>
        /// Schedules the given <paramref name="action"/> for immediate execution.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        public void ExecuteAsap(Action action)
        {
            this.InsertTask(action, DateTime.Now);
        }

        /// <summary>
        /// Schedules an action for execution after a given period of time.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        /// <param name="timeSpan">The time to wait before exection.</param>
        /// <returns>A <see cref="CancellationTokenSource">CancellationTokenSource</see> which can be used to asynchronously cancel the action.</returns>
        public CancellationTokenSource ExecuteAfter(Action action, TimeSpan timeSpan)
        {
            return this.InsertTask(action, DateTime.Now + timeSpan);
        }

        /// <summary>
        /// Schedules an action for repeated execution.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        /// <param name="timeSpan">The time to wait between executions.</param>
        /// <returns>A <see cref="CancellationTokenSource">CancellationTokenSource</see> which can be used to asynchronously cancel the action.</returns>
        public CancellationTokenSource ExecuteEvery(Action action, TimeSpan timeSpan)
        {
            ScheduledTask task = this.GetRepeatingTask(action, timeSpan);
            this.timeline.Insert(task);
            return task.CancellationTokenSource;
        }

        private ScheduledTask GetRepeatingTask(Action action, TimeSpan repeatPeriod)
        {
            // This isn't actually recursion, as insane as it sounds.
            Action executeAndInsert =
                () =>
                {
                    action();
                    ScheduledTask task = GetNewTask(
                        () => this.GetRepeatingTask(action, repeatPeriod),
                        DateTime.Now + repeatPeriod);
                    this.timeline.Insert(task);
                };
            return GetNewTask(executeAndInsert, DateTime.Now + repeatPeriod);
        }

        private CancellationTokenSource InsertTask(Action action, DateTime time)
        {
            ScheduledTask task = GetNewTask(action, time);
            this.timeline.Insert(task);
            return task.CancellationTokenSource;
        }

        private static ScheduledTask GetNewTask(Action action, DateTime time)
        {
            return new ScheduledTask(action, time);
        }
    }
}