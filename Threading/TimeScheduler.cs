using System;
using System.Threading;

namespace OpenMaple.Threading
{
    sealed partial class TimeScheduler
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
            var source = new CancellationTokenSource();
            var task = GetRepeatingTask(action, timeSpan, source.Token);
            timeline.Insert(task);
            return source;
        }

        private ScheduledTask GetRepeatingTask(Action action, TimeSpan repeatPeriod, CancellationToken token)
        {
            // This isn't actually recursion, as insane as it sounds.
            Action executeAndInsert = () =>
            {
                action();
                var task = GetNewTask(() => this.GetRepeatingTask(action, repeatPeriod, token), DateTime.Now + repeatPeriod, token);
                timeline.Insert(task);
            };
            return GetNewTask(executeAndInsert, DateTime.Now + repeatPeriod, token);
        }

        private CancellationTokenSource InsertTask(Action action, DateTime time)
        {
            var source = new CancellationTokenSource();
            var task = GetNewTask(action, time, source.Token);
            timeline.Insert(task);
            return source;
        }

        private static ScheduledTask GetNewTask(Action action, DateTime time, CancellationToken cancellationToken)
        {
            return new ScheduledTask(action, time, cancellationToken);
        }
    }
}
