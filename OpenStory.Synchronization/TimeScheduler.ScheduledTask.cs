using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace OpenStory.Synchronization
{
    internal partial class TimeScheduler
    {
        #region Nested type: ScheduledTask

        /// <summary>
        /// Represents a cancellable scheduled task.
        /// </summary>
        private sealed class ScheduledTask
        {
            private readonly Action action;

            /// <summary>
            /// The time at which this task is scheduled to execute.
            /// </summary>
            public DateTime ScheduledTime { get; private set; }

            /// <summary>
            /// Gets the <see cref="CancellationToken"/> for this task.
            /// </summary>
            public CancellationToken Cancellation { get; private set; }

            /// <summary>
            /// Initializes a new <see cref="ScheduledTask"/>, with the given action, at the given scheduled time, and with the given <see cref="CancellationToken" />.
            /// </summary>
            /// <param name="action">The action to schedule for execution.</param>
            /// <param name="scheduledTime">The time to execute the action at.</param>
            /// <param name="cancellationToken">The <see cref="CancellationToken"/> to listen to.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
            public ScheduledTask(Action action, DateTime scheduledTime, CancellationToken cancellationToken)
            {
                if (action == null)
                {
                    throw new ArgumentNullException("action");
                }

                this.action = action;
                this.ScheduledTime = scheduledTime;
                this.Cancellation = cancellationToken;
            }

            public void Execute()
            {
                this.action.Invoke();
            }
        }

        #endregion
    }
}
