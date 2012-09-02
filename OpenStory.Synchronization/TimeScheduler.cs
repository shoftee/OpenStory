using System;
using System.Threading;

namespace OpenStory.Synchronization
{
    internal sealed partial class TimeScheduler
    {
        private readonly Timeline timeline;

        public TimeScheduler()
        {
            this.timeline = new Timeline();
        }

        /// <summary>
        /// Schedules the given <paramref name="action"/> for immediate execution.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        public void ExecuteNow(Action action)
        {
            this.InsertTask(action, DateTime.Now, CancellationToken.None);
        }

        /// <summary>
        /// Schedules an action for execution after a given period of time.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        /// <param name="timeSpan">The time to wait before exection.</param>
        /// <param name="token">A <see cref="CancellationToken"/> for cancelling the task.</param>
        public void ExecuteAfter(Action action, TimeSpan timeSpan, CancellationToken token)
        {
            this.InsertTask(action, DateTime.Now + timeSpan, token);
        }

        /// <summary>
        /// Schedules an action for repeated execution.
        /// </summary>
        /// <param name="action">The action to schedule.</param>
        /// <param name="timeSpan">The time to wait between executions.</param>
        /// <param name="token">A <see cref="CancellationToken"/> for cancelling the task.</param>
        public void ExecuteEvery(Action action, TimeSpan timeSpan, CancellationToken token)
        {
            var task = this.GetRepeatingTask(action, timeSpan, token);
            this.timeline.Insert(task);
        }

        private ScheduledTask GetRepeatingTask(Action action, TimeSpan repeatPeriod, CancellationToken token)
        {
            // This isn't actually recursion, as insane as it sounds.
            Action executeAndInsert = () => this.ExecuteAndInsert(action, repeatPeriod, token);

            return GetNewTask(executeAndInsert, DateTime.Now + repeatPeriod, token);
        }

        private void ExecuteAndInsert(Action action, TimeSpan repeatPeriod, CancellationToken token)
        {
            // If the cancellation of this task has been requested, we end execution here. 
            if (token.IsCancellationRequested)
            {
                return;
            }

            // Otherwise, we execute the task and add a continuation to the timeline.
            action();

            Action getRepeatingTask =
                () => this.GetRepeatingTask(action, repeatPeriod, token);

            var task = GetNewTask(getRepeatingTask, DateTime.Now + repeatPeriod, token);

            this.timeline.Insert(task);
        }

        private void InsertTask(Action action, DateTime time, CancellationToken token)
        {
            var task = GetNewTask(action, time, token);
            this.timeline.Insert(task);
        }

        private static ScheduledTask GetNewTask(Action action, DateTime time, CancellationToken token)
        {
            return new ScheduledTask(action, time, token);
        }
    }
}
