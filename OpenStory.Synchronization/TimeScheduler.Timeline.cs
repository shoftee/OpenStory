using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenStory.Synchronization
{
    internal partial class TimeScheduler
    {
        #region Nested type: ScheduledTask

        /// <summary>
        /// Represents a cancellable scheduled task.
        /// </summary>
        private class ScheduledTask : IDisposable
        {
            private Action action;
            private CancellationToken token;

            /// <summary>
            /// Initializes a new ScheduledTask, with the given action, at the given scheduled time, and with the given <see cref="CancellationToken">CancellationToken</see>.
            /// </summary>
            /// <param name="action">The action to schedule for execution.</param>
            /// <param name="scheduledTime">The time to execute the action at.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> is <c>null</c>.</exception>
            public ScheduledTask(Action action, DateTime scheduledTime)
            {
                if (action == null) throw new ArgumentNullException("action");
                this.action = action;
                this.ScheduledTime = scheduledTime;
                this.TimeCancelled = null;

                this.CancellationTokenSource = new CancellationTokenSource();
                this.token = this.CancellationTokenSource.Token;
                this.token.Register(() => { this.TimeCancelled = DateTime.Now; });
            }

            public CancellationTokenSource CancellationTokenSource { get; private set; }

            /// <summary>
            /// The time at which this task is scheduled to execute.
            /// </summary>
            public DateTime ScheduledTime { get; private set; }

            /// <summary>
            /// Gets the timestamp for when the task was cancelled.
            /// </summary>
            public DateTime? TimeCancelled { get; private set; }

            #region IDisposable Members

            public void Dispose()
            {
                this.CancellationTokenSource.Dispose();
                GC.SuppressFinalize(this);
            }

            #endregion

            public void Execute()
            {
                this.action.Invoke();
            }
        }

        #endregion

        #region Nested type: Timeline

        /// <summary>
        /// Represents a one-way chronological list of <see cref="ScheduledTask">ScheduledTask</see> objects.
        /// </summary>
        private class Timeline
        {
            private TimelineNode front;

            /// <summary>
            /// Initializes a new empty instance of the Timeline class.
            /// </summary>
            public Timeline()
            {
                this.front = null;
            }

            /// <summary>
            /// Inserts the given task into the Timeline, after the first task which is strictly chronologically before it.
            /// </summary>
            /// <param name="task">The task to insert.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <c>null</c>.</exception>
            /// <exception cref="InvalidOperationException">Thrown if <paramref name="task"/> is already cancelled.</exception>
            public void Insert(ScheduledTask task)
            {
                if (task == null) throw new ArgumentNullException("task");
                if (task.TimeCancelled.HasValue)
                {
                    throw new InvalidOperationException("Cannot add a task that is already cancelled.");
                }

                if (this.front == null || this.front.Task.ScheduledTime > task.ScheduledTime)
                {
                    this.AddFront(new TimelineNode(task));
                    return;
                }

                TimelineNode current = this.front;
                TimelineNode next = this.front.Next;
                while (next != null && next.Task.ScheduledTime <= task.ScheduledTime)
                {
                    current = next;
                    next = current.Next;
                }

                AddAfter(current, new TimelineNode(task, current));
            }

            /// <summary>
            /// Polls all <see cref="ScheduledTask">ScheduledTask</see> objects which are due for exectution, and removes them from the front of the timeline.
            /// </summary>
            /// <returns>A list of all scheduled tasks which are due for exectuion.</returns>
            public IEnumerable<ScheduledTask> GetAllDue()
            {
                var tasks = new List<ScheduledTask>();

                DateTime now = DateTime.Now;
                TimelineNode node = this.front;
                while (node != null && node.Task.ScheduledTime <= now)
                {
                    tasks.Add(node.Task);
                    node = node.Next;
                }
                this.front = node;
                return tasks;
            }

            private void AddFront(TimelineNode node)
            {
                node.Next = this.front;
                this.front = node;
            }

            private static void AddAfter(TimelineNode node, TimelineNode newNode)
            {
                if (node == null) throw new ArgumentNullException("node");
                if (newNode == null) throw new ArgumentNullException("newNode");

                if (node.Next != null)
                {
                    newNode.Next = node.Next;
                }
                node.Next = newNode;
            }

            #region Nested type: TimelineNode

            /// <summary>
            /// Represents a single-link node for a Timleine.
            /// </summary>
            private class TimelineNode
            {
                /// <summary>
                /// Initializes a new instance of the TimelineNode class, with the given scheduled task.
                /// </summary>
                /// <param name="task">The task for this TimelineNode.</param>
                /// <param name="next">Optional. The TimelineNode to use as a next node reference. The default value is <c>null</c>.</param>
                public TimelineNode(ScheduledTask task, TimelineNode next = null)
                {
                    this.Task = task;
                    this.Next = next;
                }

                /// <summary>
                /// The task for this TimelineNode.
                /// </summary>
                public ScheduledTask Task { get; private set; }

                /// <summary>
                /// A reference to the next TimelineNode in the Timeline.
                /// </summary>
                public TimelineNode Next { get; set; }
            }

            #endregion
        }

        #endregion
    }
}